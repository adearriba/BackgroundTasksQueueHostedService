# QueueHostedService
Basically to add a new IBackgroundTask to the QueuedHostedService, you will use the Channel Writer to write to the Channel, which is configured as a Singleton. The **QueuedHostedService** will use the Channel Reader to read one by one, awaiting each IBackgroundTask to ends.

```csharp
//Adds a new task to the Channel
_channel.Writer.WriteAsync(new BackgroundTask(_serviceProvider));

//Reads a task from the Channel or awaits for a task to be added
var currentTask = await _channel.Reader.ReadAsync();
```
