﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Ex3.Models
{
    public class Server
    {
        //flag to check if need to stop
        private bool shouldStop;
        private float lon;
        private float lat;
        TcpListener server;
        TcpClient clientSocket;
        Thread thread;

        public Server()
        {
            shouldStop = false;
            lon = 0.0f;
            lat = 0.0f;

        }

        // property of Lon
        public float Lon
        {
            get { return lon; }
            set
            {
                lon = value;

            }
        }

        // property of Lat
        public float Lat
        {
            get { return lat; }
            set
            {
                lat = value;

            }
        }

        private static string readLine(BinaryReader reader)
        {
            // storage the line
            char[] buffer = new char[1024];
            int i = 0;
            // the end of line
            char end = '\0';
            // read untill the end of line
            while (i < 1024 && end != '\n')
            {
                char input = reader.ReadChar();
                buffer[i] = input;
                end = buffer[i];
                i++;
            }

            return new string(buffer);
        }
        //open the server
        public void openServer(string ip,int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip),port);
            this.server = new TcpListener(ep);
            // open the server
            server.Start();
            // wait for connect
            this.clientSocket = server.AcceptTcpClient();
            // after connection- start listen to the flight.
            this.thread = new Thread(() => listenFlight(server, clientSocket));
            thread.Start();
        }
        //listen to the airplane
        private void listenFlight(TcpListener server, TcpClient clientSocket)
        {
            NetworkStream stream = clientSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            DateTime start = DateTime.UtcNow;
            string inputLine;
            string[] splitStr;

            while (!shouldStop)
            {
                inputLine = readLine(reader);
                if (Convert.ToInt32((DateTime.UtcNow - start).TotalSeconds) < 90)
                {
                    continue;
                }
                // take from the flight only the lon and the lat
                splitStr = inputLine.Split(',');
                //FlightBoardViewModel.Instance.Lon = float.Parse(splitStr[0]);
                //FlightBoardViewModel.Instance.Lat = float.Parse(splitStr[1]);
            }


        }


        public void getLocation()
        {
            NetworkStream stream = clientSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            DateTime start = DateTime.UtcNow;
            string inputLine;
            string[] splitStr;

            inputLine = readLine(reader);
            // take from the flight only the lon and the lat
            splitStr = inputLine.Split(',');
            //FlightBoardViewModel.Instance.Lon = float.Parse(splitStr[0]);
            //FlightBoardViewModel.Instance.Lat = float.Parse(splitStr[1]);
            Lon = float.Parse(splitStr[0]);
            Lat = float.Parse(splitStr[1]);

        }
        // close the socket
        public void close()
        {
            this.shouldStop = true;
            this.thread.Abort();
            this.clientSocket.Close();
            this.server.Stop();

        }
    }

}