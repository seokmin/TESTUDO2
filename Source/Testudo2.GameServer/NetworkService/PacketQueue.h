#pragma once

#include <mutex>
#include <queue>

#include "Packet.h"

namespace SoraeNet
{
	class PacketQueue
	{
	public:
		PacketQueue() = default;
		~PacketQueue();
		PacketQueue(PacketQueue&) = delete;

		std::shared_ptr<Packet> Dequeue();
		void					Enqueue(std::shared_ptr<Packet> packet);

	private:
		std::queue<std::shared_ptr<Packet>>*	_packetQueue = new std::queue<std::shared_ptr<Packet>>();
		std::mutex*								_mutex = new std::mutex();
	};
}