using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ConditionSystem
{

    public class ConditionUI_Power : ConditionUiBase
    {
      //  enum PowerConditionMode {less,more}
        [SerializeField] InputField PowerInput;
        private WaypointConditionValues_Float _targetValues;


        public void SetTarget(WaypointConditionValues_Float target)
        {
            _targetValues = target;
            SetUIValues(target);
        }

        public void SetUIValues(WaypointConditionValues_Float values)
        {
            PowerInput.text = "" + values.value;
        }

        public void SetValue(InputField field)
        {
            _targetValues.value = float.Parse(field.text);

        }
    }
}
