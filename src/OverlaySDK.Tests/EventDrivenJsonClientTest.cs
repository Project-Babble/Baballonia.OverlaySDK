using System.Text.Json;
using OverlaySDK;
using OverlaySDK.Packets;

namespace OverlaySDK.Tests;

public class EventDrivenJsonClientTest
{
    
    [Fact]
    public async Task Integration()
    {
        var socketFactory = new SocketFactory();

        Packet<RunFixedLenghtRoutinePacket> testPacket =
            new Packet<RunFixedLenghtRoutinePacket>(new RunFixedLenghtRoutinePacket("ballz"));

        var t1 = Task.Run(() =>
        {
            List<JsonDocument> docs = new();
            var tcp1 = new EventDrivenTcpClient(socketFactory.CreateServer("127.0.0.1", 1234));
            var json1 = new EventDrivenJsonClient(tcp1);
            json1.DataReceived += doc =>
            {
                docs.Add(doc);
            };
            json1.Send(testPacket);
            json1.Send(testPacket);
            json1.Send(testPacket);

            Thread.Sleep(30);

            return docs;
        });
        
        var t2 = Task.Run(() =>
        {
            List<JsonDocument> docs = new();
            var tcp2 = new EventDrivenTcpClient(socketFactory.CreateClient("127.0.0.1", 1234));
            var json2 = new EventDrivenJsonClient(tcp2);
            json2.DataReceived += doc =>
            {
                docs.Add(doc);
            };

            json2.Send(testPacket);
            json2.Send(testPacket);

            Thread.Sleep(30);

            return docs;
        });

        var res1 = await t1;
        var res2 = await t2;

        Assert.Equal(2, res1.Count);
        Assert.Equal(3, res2.Count);

        Assert.Equal(testPacket.PacketName, res1.First().Deserialize<IncomingPacket>()!.PacketName);
        Assert.Equal(testPacket.PacketName, res2.First().Deserialize<IncomingPacket>()!.PacketName);
    }
}