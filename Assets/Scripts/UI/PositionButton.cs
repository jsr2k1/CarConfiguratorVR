using UnityEngine;
using Leap.Unity.Interaction;

	public class PositionButton : MonoBehaviour
{
	public InteractionSlider slider;
	public float percentage = 20;
	public bool vertical = true;

	public void MoveSlider(bool up)
	{
		int sense = (up) ? 1 : -1;

		if(vertical)
		{
			slider.VerticalSliderPercent += sense * percentage / 100f;
			slider.VerticalSliderPercent = Mathf.Clamp(slider.VerticalSliderPercent, 0f, 1f);

			if(up){
				Messenger<int>.Broadcast(MessengerEventsEnum.MOVE_CAMERA, 1);
			} else{
				Messenger<int>.Broadcast(MessengerEventsEnum.MOVE_CAMERA, -1);
			}
		}
		else{
			slider.HorizontalSliderPercent += sense * percentage / 100f;
			slider.HorizontalSliderPercent = Mathf.Clamp(slider.HorizontalSliderPercent, 0f, 1f);
		}
	}
}
