using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes.Shade
{
    public class ShadePropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _propSetter;

        // Shade Mode
        public MaterialProperty ShadeMode;

        // Pos And Blur
        public MaterialProperty UseFirstShade;
        public MaterialProperty FirstShadeMap;
        public MaterialProperty FirstShadeColor;
        public MaterialProperty FirstShadeBorderPos;
        public MaterialProperty FirstShadeBorderBlur;

        public MaterialProperty UseExFirstShade;
        public MaterialProperty ExFirstShadeColor;
        public MaterialProperty ExFirstShadeWidth;

        public MaterialProperty UseSecondShade;
        public MaterialProperty SecondShadeMap;
        public MaterialProperty SecondShadeColor;
        public MaterialProperty SecondShadeBorderPos;
        public MaterialProperty SecondShadeBorderBlur;

        // Ramp
        public MaterialProperty UseRampShade;
        public MaterialProperty RampShadeMap;

        // Control Map
        public MaterialProperty UseShadeControlMap;
        public MaterialProperty ShadeControlMap;
        public MaterialProperty ShadeControlMapIntensity;

        public ShadePropertiesContainer(PropertySetter propSetter)
        {
            _propSetter = propSetter;
        }

        public void Set(MaterialProperty[] materialProperties)
        {
            _propSetter.Set(this, materialProperties);
        }
    }
}
