/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2017.                                 *
 * Leap Motion proprietary and  confidential.                                 *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using Leap.Unity.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Attachments {

  /// <summary>
  /// Simple container class for storing a reference to the attachment point this
  /// transform corresponds to within an AttachmentHand. Also contains mappings from
  /// a single AttachmentPointFlags flag constant to the relevant bone on a Leap.Hand;
  /// these mappings can be accessed statically via GetLeapHandPointData().
  /// 
  /// Can also be used to refer to a single AttachmentPointFlags flag constant (implicit conversion).
  /// </summary>
  [AddComponentMenu("")]
  [ExecuteInEditMode]
  public class AttachmentPointBehaviour : MonoBehaviour {

    [Tooltip("The AttachmentHand associated with this AttachmentPointBehaviour. AttachmentPointBehaviours "
           + "should be beneath their AttachmentHand object in the hierarchy.")]
    [Disable]
    public AttachmentHand attachmentHand;

    [Tooltip("To change which attachment points are available on an AttachmentHand, refer to the "
           + "inspector for the parent AttachmentHands object.")]
    [Disable]
    public AttachmentPointFlags attachmentPoint;

    void OnValidate() {
      if (!attachmentPoint.IsSinglePoint() && attachmentPoint != AttachmentPointFlags.None) {
        Debug.LogError("AttachmentPointBehaviours should refer to a single attachmentPoint flag.", this.gameObject);
        attachmentPoint = AttachmentPointFlags.None;
      }
    }

    void OnDestroy() {
      if (attachmentHand != null) {
        attachmentHand.notifyPointBehaviourDeleted(this);
      }
    }

    public static implicit operator AttachmentPointFlags(AttachmentPointBehaviour p) {
      if (p == null) return AttachmentPointFlags.None;
      return p.attachmentPoint;
    }

    public void SetTransformUsingHand(Leap.Hand hand) {
        if (hand == null) {
            this.transform.gameObject.SetActive(false);
            //Debug.LogError("Unable to set transform with a null hand.", this.gameObject);
            return;
        }
        this.transform.gameObject.SetActive(true);

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        GetLeapHandPointData(hand, this.attachmentPoint, out position, out rotation);

            
        this.transform.position = position;
        this.transform.rotation = rotation;
        //this.transform.position = Vector3.Lerp(this.transform.position, position, 0.5f);
        //Vector3 r = rotation.eulerAngles;
        //      r.z = 0;
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, 0.05f);
    }

    public static void GetLeapHandPointData(Leap.Hand hand, AttachmentPointFlags singlePoint, out Vector3 position, out Quaternion rotation) {
      position = Vector3.zero;
      rotation = Quaternion.identity;

      if (!singlePoint.IsSinglePoint()) {
        Debug.LogError("Cannot get attachment point data for an AttachmentPointFlags argument consisting of more than one set flag.");
        return;
      }

      switch (singlePoint) {
        case AttachmentPointFlags.None:
          Debug.LogError("Unable to set transform; this AttachmentPointBehaviour does not have its attachment point flag set.");
          return;

        case AttachmentPointFlags.Wrist:
          position = hand.WristPosition.ToVector3();
          rotation = hand.Arm.Basis.rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.Palm:
          position = hand.PalmPosition.ToVector3();
          rotation = hand.Basis.rotation.ToQuaternion();
          break;

        case AttachmentPointFlags.ThumbProximalJoint:
          position = hand.Fingers[0].bones[1].NextJoint.ToVector3();
          rotation = hand.Fingers[0].bones[2].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.ThumbDistalJoint:
          position = hand.Fingers[0].bones[2].NextJoint.ToVector3();
          rotation = hand.Fingers[0].bones[3].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.ThumbTip:
          position = hand.Fingers[0].bones[3].NextJoint.ToVector3();
          rotation = hand.Fingers[0].bones[3].Rotation.ToQuaternion();
          break;

        case AttachmentPointFlags.IndexKnuckle:
          position = hand.Fingers[1].bones[0].NextJoint.ToVector3();
          rotation = hand.Fingers[1].bones[1].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.IndexMiddleJoint:
          position = hand.Fingers[1].bones[1].NextJoint.ToVector3();
          rotation = hand.Fingers[1].bones[2].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.IndexDistalJoint:
          position = hand.Fingers[1].bones[2].NextJoint.ToVector3();
          rotation = hand.Fingers[1].bones[3].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.IndexTip:
          position = hand.Fingers[1].bones[3].NextJoint.ToVector3();
          rotation = hand.Fingers[1].bones[3].Rotation.ToQuaternion();
          break;

        case AttachmentPointFlags.MiddleKnuckle:
          position = hand.Fingers[2].bones[0].NextJoint.ToVector3();
          rotation = hand.Fingers[2].bones[1].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.MiddleMiddleJoint:
          position = hand.Fingers[2].bones[1].NextJoint.ToVector3();
          rotation = hand.Fingers[2].bones[2].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.MiddleDistalJoint:
          position = hand.Fingers[2].bones[2].NextJoint.ToVector3();
          rotation = hand.Fingers[2].bones[3].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.MiddleTip:
          position = hand.Fingers[2].bones[3].NextJoint.ToVector3();
          rotation = hand.Fingers[2].bones[3].Rotation.ToQuaternion();
          break;

        case AttachmentPointFlags.RingKnuckle:
          position = hand.Fingers[3].bones[0].NextJoint.ToVector3();
          rotation = hand.Fingers[3].bones[1].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.RingMiddleJoint:
          position = hand.Fingers[3].bones[1].NextJoint.ToVector3();
          rotation = hand.Fingers[3].bones[2].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.RingDistalJoint:
          position = hand.Fingers[3].bones[2].NextJoint.ToVector3();
          rotation = hand.Fingers[3].bones[3].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.RingTip:
          position = hand.Fingers[3].bones[3].NextJoint.ToVector3();
          rotation = hand.Fingers[3].bones[3].Rotation.ToQuaternion();
          break;

        case AttachmentPointFlags.PinkyKnuckle:
          position = hand.Fingers[4].bones[0].NextJoint.ToVector3();
          rotation = hand.Fingers[4].bones[1].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.PinkyMiddleJoint:
          position = hand.Fingers[4].bones[1].NextJoint.ToVector3();
          rotation = hand.Fingers[4].bones[2].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.PinkyDistalJoint:
          position = hand.Fingers[4].bones[2].NextJoint.ToVector3();
          rotation = hand.Fingers[4].bones[3].Rotation.ToQuaternion();
          break;
        case AttachmentPointFlags.PinkyTip:
          position = hand.Fingers[4].bones[3].NextJoint.ToVector3();
          rotation = hand.Fingers[4].bones[3].Rotation.ToQuaternion();
          break;
      }
    }

  }

}
