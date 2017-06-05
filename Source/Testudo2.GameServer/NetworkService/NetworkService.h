#pragma once

#include <stack>
#include <memory>
#include <unordered_map>

#include "Session.h"
#include "PacketQueue.h"

namespace SoraeNet
{
	class NetService
	{
#pragma region inner classes..
		struct Configuration
		{
			unsigned	_maxSessionCount;
			unsigned	_recvBufferSizePerSession;
			unsigned short	_portNum;
			unsigned short	_backLogCount;

			class Factory
			{
			public:
				static Configuration CreateFromDefault()
				{
					auto instance = Configuration();
					instance._maxSessionCount = 1000;
					// FIXME(sorae): 1mb size is temporal. need to find appropriate size of buffer
					instance._recvBufferSizePerSession = 1024*1024;
					instance._portNum = 38246;
					instance._backLogCount = 128;

					return std::move(instance);
				}
			};
		};

		struct IOInfo
		{
		public:
			OVERLAPPED			_overlapped;
			WSABUF				_wsaBuf; // WSARecv¿ë
			enum class IOMode : short
			{
				READ = 0,
				WRITE = 1
			} _ioMode;
			unsigned			_sessionId;
		};
#pragma endregion

	public:
		NetService(const NetService::Configuration& configuration);
		NetService();
		~NetService() = default;

		std::shared_ptr<PacketQueue>			GetReceivePacketQueue();
		std::shared_ptr<PacketQueue>			GetSendPacketQueue();

		void									Start();

	public:


	private:
		NetService(NetService&)					= delete;
		NetService(NetService&&)				= delete;
		
		void									listenForNewSession();

		void									bindSessionToIOCP(std::shared_ptr<Session> targetSession);
		void									workerThreadFunc();


	private:
		const Configuration						_configuration;

		std::shared_ptr<PacketQueue>			_recvPacketQueue = std::make_shared<PacketQueue>();
		std::shared_ptr<PacketQueue>			_sendPacketQueue = std::make_shared<PacketQueue>();

		std::stack<std::shared_ptr<Session>>*													_sessionPool = nullptr;
		std::unique_ptr<std::unordered_map<decltype(Session::_id), std::shared_ptr<Session>>>	_connectedSessions = nullptr;

		std::mutex								_sessionPoolLocker;

		HANDLE									_handleForIOCP = nullptr;
		SOCKET									_listenSocket = -1;


	};
}