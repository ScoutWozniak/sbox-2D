using System.Linq;
using Sandbox;
using Sandbox.Citizen;

public sealed class DuccBlaster : Component
{
	[Property] public GameObject blaster { get; set; }
	[Property] public GameObject bullet { get; set; }
	[Property] public GameObject body { get; set; }
	[Property] public CitizenAnimationHelper animationHelper { get; set; }
	Attack attack => Scene.GetAllComponents<Attack>().FirstOrDefault();
	protected override void OnUpdate()
	{
		if (Input.Pressed("attack1") && attack.DuccBlasterEnabled)
		{
			animationHelper.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
			var bulletGo = bullet.Clone(body.Transform.Position + body.Transform.Rotation.Up * 45f + body.Transform.Rotation.Forward * 100f, body.Transform.Rotation);
			var rb = bulletGo.Components.Get<Rigidbody>();
			rb.Velocity = body.Transform.Rotation.Forward * 1000;
		}

	}
	
}