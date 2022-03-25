using Godot;
using static Settings;
using Discord;
using System;

public class Main: Node {
	public readonly static long CLIENT_ID = 956699877453226044;
	public static Discord.Discord CLIENT {get;private set;}

	public override void _Ready() {
		try {
			CLIENT = new Discord.Discord(CLIENT_ID, (ulong)Discord.CreateFlags.NoRequireDiscord);
			ActivityManager manager = CLIENT.GetActivityManager();
			Activity activity = new Activity {
				State = "Testing Hellstrafe"
			};
			manager.UpdateActivity(activity, (res) => {});
		} catch (Exception ignored) {}
	}

	public override void _Process(float delta) {
		if (debug && Input.IsActionPressed("ui_cancel")) {
			if (CLIENT != null)
				CLIENT.Dispose();
			GetTree().Quit();
		}

		updateDiscordPresence();
	}

	public static void updateDiscordPresence() {
		if (CLIENT != null)
			CLIENT.RunCallbacks();
	}
}
