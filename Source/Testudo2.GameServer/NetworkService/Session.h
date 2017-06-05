#include <winsock2.h>


namespace SoraeNet
{
	class Session
	{
	public:
		bool			IsConnected() { return _socket > 0 ? true : false; }

	public:
		unsigned		_id = 0;
		SOCKET			_socket = -1;
		sockaddr_in		_socketAddress;
		char*			_recvBuffer = nullptr;

	private:
	private:
	};
}