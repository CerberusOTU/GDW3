#include <iostream>
#include <WS2tcpip.h>
#include <string>

#pragma comment (lib, "ws2_32.lib")

using namespace std; //Saves from typing std::

void main(int argc, char* argv[]) //We can pass in a command line option
{
	//Startup Winsock
	WSADATA data;
	WORD version = MAKEWORD(2, 2);
	int wsOk = WSAStartup(version, &data);
	if (wsOk != 0)
	{
		cout << "Can't start Winsock!" << wsOk;
		return;
	}

	//Create a hint structure for the server
	sockaddr_in server;
	server.sin_family = AF_INET;
	server.sin_port = htons(54000);

	inet_pton(AF_INET, "127.0.0.1", &server.sin_addr);

	//Socket creation
	SOCKET out = socket(AF_INET, SOCK_DGRAM, 0);

	//Write out to socket
	string s(argv[1]);
	int sendOK = sendto(out, s.c_str(), s.size() + 1, 0, (sockaddr*)&server, sizeof(server));

	if (sendOK == SOCKET_ERROR)
	{
		cout << "That didn't work!" << WSAGetLastError() << endl;
	}

	//Close socket
	closesocket(out);
	//Close Winsock
	WSACleanup();
}