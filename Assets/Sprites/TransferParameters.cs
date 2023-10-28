using UnityEngine;

public class TransferParameters : MonoBehaviour
{
    public Animator parentAnimator;
    public Animator childAnimator;

    void Update()
    {
        AnimatorControllerParameter[] parameters = parentAnimator.parameters;

        foreach (AnimatorControllerParameter parameter in parameters)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    childAnimator.SetBool(parameter.name, parentAnimator.GetBool(parameter.name));
                    break;

                case AnimatorControllerParameterType.Float:
                    childAnimator.SetFloat(parameter.name, parentAnimator.GetFloat(parameter.name));
                    break;

                case AnimatorControllerParameterType.Int:
                    childAnimator.SetInteger(parameter.name, parentAnimator.GetInteger(parameter.name));
                    break;

                case AnimatorControllerParameterType.Trigger:
                    if (parentAnimator.GetBool(parameter.name))
                    {
                        childAnimator.SetTrigger(parameter.name);
                    }
                    break;
            }
        }
    }
}