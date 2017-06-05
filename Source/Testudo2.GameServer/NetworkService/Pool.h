#pragma once

#include <stack>
#include <mutex>

template<class T>
class ObjectPool<T>
{
public:
	ObjectPool<T>() = delete;

	ObjectPool<T>(unsigned size, bool needsThreadSafe)
	{
		_pool = new std::stack<T>();
		if (needsThreadSafe)
			_mutex = new std::mutex();

		for (auto i = 0u; i < size; ++i)
		{
			_pool.push(new T());
		}
	}

	~ObjectPool<T>()
	{
		if (_pool != nullptr)
			delete _pool;
		if (_mutex != nullptr)
			delete _mutex;
	}


	

public:

private:
	
private:
	std::mutex*		_mutex	= nullptr;
	std::stack<T>*	_pool	= nullptr;

};