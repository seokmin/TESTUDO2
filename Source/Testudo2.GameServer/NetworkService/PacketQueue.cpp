#include "PacketQueue.h"

#include <thread>
#include <chrono>

using namespace std;

namespace SoraeNet
{
	PacketQueue::~PacketQueue()
	{
		delete(_packetQueue);
		_packetQueue = nullptr;
		delete(_mutex);
		_mutex = nullptr;
	}

	// AUTHOR(sorae): When queue is occupied by other thread, this function will be blocked.
	std::shared_ptr<SoraeNet::Packet> PacketQueue::Dequeue()
	{
		_mutex->lock();
		{
			while (_packetQueue->empty())
				this_thread::sleep_for(chrono::milliseconds::zero());

			auto instance = _packetQueue->front();
			_packetQueue->pop();
			
			return instance;
		}
		_mutex->unlock();
	}

	void PacketQueue::Enqueue(std::shared_ptr<Packet> packet)
	{
		_mutex->lock();
		{
			_packetQueue->push(packet);
		}
		_mutex->unlock();
	}
}