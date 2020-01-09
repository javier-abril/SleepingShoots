#pragma strict
var footsource : AudioSource;
function Start () {

}

function Update () 
{
	

}
function OnTriggerEnter (other : Collider) 
{
	if (!footsource.isPlaying)
	{
	
		footsource.Play();
	}
}