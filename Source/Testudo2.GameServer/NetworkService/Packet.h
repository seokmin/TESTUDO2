#pragma once
namespace SoraeNet
{
	struct Packet
	{
		enum class Type : unsigned short
		{
			Error = 0,


		};

		struct Header
		{
			unsigned		_contentSize	= 0;
			unsigned short	_packetId		= 0;
		};

		Packet(decltype(Header::_packetId) packetID, char* contentPos)
		{

		}

		~Packet()
		{
			if (_contentPosition != nullptr)
				delete[] _contentPosition;
		}
		// NOTE(sorae): when packet is received packet, id means source
		//				on opposite case, it means destination
		unsigned	_sessionId = -1;
		char*		_contentPosition;

		static class Factory
		{
		public:
			static std::shared_ptr<Packet> Create(decltype(Header::_packetId) packetId, char* contentPos)
			{
				Packet* instance = nullptr;

				switch (packetId)
				{
				}
			}
		};
	};
}