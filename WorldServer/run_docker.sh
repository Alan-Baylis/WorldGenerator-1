sudo docker run --name worldserver -h worldserver -p 8084-8087:8084-8087 -i -t -v /mnt/archive/Programming/Cloud/WorldGenerator/WorldServer:/opt/WorldServer mono:4.2.1.102 bash
#sudo docker build -t my_elixir_server .
#sudo docker run --name elixirserver -i -t my_elixir_server /opt/ElixirMessagingServer/ElixirMessagingServer
sudo docker rm worldserver
