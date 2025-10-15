using OverlaySDK.Packets;

namespace OverlaySDK;

public abstract class PacketHandlerAdapter
{
    public virtual void OnStartRoutine(RunFixedLenghtRoutinePacket routine)
    {
    }

    public virtual void OnStartRoutine(RunVariableLenghtRoutinePacket routine)
    {
    }

    public virtual void OnHmdPositionalData(HmdPositionalDataPacket positionalData)
    {
    }

    public virtual void OnTrainerProgressReport(TrainerProgressReportPacket progressReport)
    {
    }

    public virtual void OnRoutineFinishedPacket(RoutineFinishedPacket packet)
    {
    }

    public virtual void OnTermination()
    {
    }
}