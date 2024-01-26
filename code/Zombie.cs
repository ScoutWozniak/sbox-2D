using System.Linq;
using Sandbox;
using Sandbox.Citizen;

public sealed class Zombie : Component
{
	CitizenAnimationHelper citizenAnimationHelper => Components.Get<CitizenAnimationHelper>(FindMode.InSelf);
	[Property] public GameObject body { get; set; }
	[Property] public CharacterController characterController { get; set; }
	PlayerController controller => Scene.GetAllComponents<PlayerController>().FirstOrDefault();
	[Property] public float Speed { get; set; }
	public Vector3 target;
	public Vector3 WishVelocity;
	protected override void OnStart()
	{
		citizenAnimationHelper.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
	}
	protected override void OnUpdate()
	{
		target = controller.GameObject.Transform.Position;
		BuildWishVelocity();
		UpdateMovement();
		UpdateAnimations();
	}

	void BuildWishVelocity()
	{
		WishVelocity = (target - Transform.Position).Normal;

		WishVelocity = WishVelocity.Normal;
		WishVelocity = WishVelocity * Speed;
	}

	void UpdateMovement()
	{
		
		characterController.ApplyFriction(GetFriction());

		if (characterController.IsOnGround)
		{
			characterController.Accelerate(WishVelocity);
			characterController.Velocity = characterController.Velocity.WithZ(0);
		}
		characterController.Move();
	}
	float GetFriction()
	{
		if ( characterController.IsOnGround ) return 6.0f;

		return 0.2f;
	}
	void UpdateAnimations()
	{
		// Rotate body towards target
		if ( target != Vector3.Zero )
		{
			var targetRot = Rotation.LookAt( target.WithZ( Transform.Position.z ) - Transform.Position, Vector3.Up );
			body.Transform.Rotation = Rotation.Slerp( body.Transform.Rotation, targetRot, Time.Delta * 10f );
		}

		citizenAnimationHelper.WithWishVelocity( WishVelocity );
		citizenAnimationHelper.WithVelocity( characterController.Velocity );
		
	}
}
