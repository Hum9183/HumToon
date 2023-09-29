using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.Emission
{
    public class EmissionPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        public MaterialProperty UseEmission;
        public MaterialProperty EmissionMap;
        public MaterialProperty EmissionColor;
        public MaterialProperty EmissionIntensity;
        public MaterialProperty EmissionFactorR;
        public MaterialProperty EmissionFactorG;
        public MaterialProperty EmissionFactorB;
        public MaterialProperty OverrideEmissionColor;

        public EmissionPropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set(MaterialProperty[] materialProperties)
        {
            _propSetter.Set(this, materialProperties);
        }
    }
}
