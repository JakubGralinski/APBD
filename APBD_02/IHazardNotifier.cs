namespace ContainerLoadingManagement;

// Interface for sending hazard notifications.
public interface IHazardNotifier
{
    void NotifyHazard(string message);
}