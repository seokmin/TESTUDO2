#include <future>


#include "NetworkService.h"

#include "PacketQueue.h"

namespace SoraeNet
{
	using namespace std;

	NetService::NetService(const Configuration& config) : _configuration(config)
	{
#pragma region task definitions..
		auto createSessionPool = [](const unsigned sessionCount, const unsigned recvBufferSize)
		{
			auto pool = new std::stack<std::shared_ptr<Session>>();

			for (auto i = 0u; i < sessionCount; ++i)
			{
				auto newSession = std::make_shared<Session>();
				newSession->_id = i;
				newSession->_recvBuffer = new char[recvBufferSize];
				pool->emplace(std::move(newSession));
			}
			return std::move(pool);
		};
		auto createListenSocket = [](auto portNumber)
		{
			auto listenSocket = WSASocketW(AF_INET, SOCK_STREAM, 0, nullptr, 0, WSA_FLAG_OVERLAPPED);
			sockaddr_in socketAddress;
			ZeroMemory(&socketAddress, sizeof(socketAddress));
			socketAddress.sin_family = AF_INET;
			socketAddress.sin_addr.s_addr = htonl(INADDR_ANY);
			socketAddress.sin_port = htons(portNumber);

			bind(listenSocket, (sockaddr*)&socketAddress, sizeof(socketAddress));
			return listenSocket;
		};
#pragma endregion

		_sessionPool   = createSessionPool(config._maxSessionCount, config._recvBufferSizePerSession);
		_handleForIOCP = CreateIoCompletionPort(INVALID_HANDLE_VALUE, nullptr, 0, 0);
		_listenSocket  = createListenSocket(config._portNum);
		_connectedSessions = make_unique<decltype(_connectedSessions)::element_type>();
	}

	// NOTE(sorae): Network configuration will be set to default
	NetService::NetService() : NetService(Configuration::Factory::CreateFromDefault())
	{
	}

	std::shared_ptr<PacketQueue> NetService::GetReceivePacketQueue()
	{
		return _recvPacketQueue;
	}

	std::shared_ptr<PacketQueue> NetService::GetSendPacketQueue()
	{
		return _sendPacketQueue;
	}

	void NetService::Start()
	{
#pragma region task definitions..
		auto getNumberOfProcessors = []
		{
			auto sysInfo = SYSTEM_INFO();
			GetSystemInfo(&sysInfo);
			return sysInfo.dwNumberOfProcessors;
		};
		auto startWorkerThreads = [](auto numberOfThreads)
		{
			for (auto i = 0u; i < numberOfThreads; ++i)
			{
				// TODO(sorae): start worker threads..
			}
		};
#pragma endregion

		listen(_listenSocket, _configuration._backLogCount);

		auto listenThread = new std::thread([this] {listenForNewSession(); });
		listenThread->detach();

		startWorkerThreads(getNumberOfProcessors() * 2);
	}

	void NetService::workerThreadFunc()
	{
#define PACKET_HEADER_SIZE sizeof(Packet::Header)

		DWORD transferedByte = 0;
		IOInfo* finishedIO = nullptr;

		while (true)
		{
			GetQueuedCompletionStatus(_handleForIOCP, &transferedByte,
				nullptr, (LPOVERLAPPED*)&finishedIO, INFINITE);

			auto session = _connectedSessions->at(finishedIO->_sessionId);

			if (finishedIO->_ioMode == IOInfo::IOMode::READ)
			{
				// NOTE(sorae): transferedByte will set to 0 when session is closed
				if (transferedByte == 0)
				{
					closesocket(session->_socket);
					ZeroMemory(session.get(), sizeof(session));
					_sessionPoolLocker.lock();
					_sessionPool->push(session);
					_sessionPoolLocker.unlock();

					// TODO(sorae): send session closed packet to logic layer

					continue;
				}

				auto headerPos = session->_recvBuffer;
				// NOTE(sorae): receivedPos means start position of new datas, not start position of receive buffer.
				//				There may be residual data that has not been processed in the previous phase.
				auto receivedPos = finishedIO->_wsaBuf.buf;
				// NOTE(sorae): size of not processed data == end position - start position
				auto totalDataSizeInBuf		= (receivedPos + transferedByte) - session->_recvBuffer;
				auto remainingDataSizeInBuf = totalDataSizeInBuf;

				// NOTE(sorae): packet José loop
				while (remainingDataSizeInBuf >= PACKET_HEADER_SIZE)
				{
					auto header = reinterpret_cast<Packet::Header*>(headerPos);
					
					// if there is enough data to make one packet
					if (PACKET_HEADER_SIZE + header->_contentSize >= remainingDataSizeInBuf)
					{
						auto packetId = header->_packetId;

					}
				}
			}
		}
	}

	void NetService::listenForNewSession()
	{
#pragma region task definitions..
#pragma endregion

		while (true)
		{
			auto clientAddress = sockaddr_in{};
			int addrLen = sizeof(clientAddress);

			auto newSocket = accept(_listenSocket, (sockaddr*)&clientAddress, &addrLen);

			// TODO(sorae): 동접자 초과 시 동접자 초과 메시지를 클라에 보내주도록 변경해야 함.
			while (_sessionPool->empty())
				Sleep(1000);

			_sessionPoolLocker.lock();
			auto newSession = _sessionPool->top();
			_sessionPool->pop();
			_sessionPoolLocker.unlock();

			newSession->_socketAddress = clientAddress;
			newSession->_socket = newSocket;

			auto ioInfo = new IOInfo();
			ZeroMemory(&ioInfo->_overlapped, sizeof(&ioInfo->_overlapped));
			ioInfo->_wsaBuf.buf = newSession->_recvBuffer;
			ioInfo->_wsaBuf.len = _configuration._recvBufferSizePerSession;
			ioInfo->_ioMode = IOInfo::IOMode::READ;
			ioInfo->_sessionId = newSession->_id;

			bindSessionToIOCP(newSession);

			DWORD receivedSize = 0;
			DWORD flags = 0;

			// NOTE(sorae): reserve to receive
			WSARecv(newSession->_socket, &ioInfo->_wsaBuf, 1, &receivedSize, &flags, &ioInfo->_overlapped, nullptr);

		}
	}

	void NetService::bindSessionToIOCP(std::shared_ptr<Session> targetSession)
	{
		CreateIoCompletionPort((HANDLE)targetSession->_socket, _handleForIOCP, static_cast<ULONG_PTR>(NULL), 0);
	}
}