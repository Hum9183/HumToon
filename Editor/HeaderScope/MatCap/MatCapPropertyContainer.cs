using UnityEditor;

namespace HumToon.Editor
{
    public class MatCapPropertyContainer : IPropertyContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty UseMatCap;
        public MaterialProperty MatCapMap;
        public MaterialProperty MatCapColor;

        public MatCapPropertyContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set()
        {
            _propSetter.Set(this);
        }
    }
}
