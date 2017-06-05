#include <winsock2.h>
namespace NetService
{
	struct SessionInfo
	{
		unsigned	_id = 0;
		SOCKET			_socket = -1;
		sockaddr_in		_sockAddr;
		char*			_receiveBuffer = nullptr;
		bool			_isConnected() { return _socket > 0 ? true : false; }
	};
}