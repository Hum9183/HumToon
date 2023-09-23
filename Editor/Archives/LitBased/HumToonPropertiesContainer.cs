using UnityEditor;

namespace HumToon.Editor.LitBased
{
    public class HumToonPropertiesContainer : IPropertiesContainer
    {
        private readonly PropertySetter _matPropSetter;

        public MaterialProperty SurfaceType;
        public MaterialProperty BlendMode;
        public MaterialProperty BlendModePreserveSpecular;
        public MaterialProperty CullMode;
        // public MaterialProperty ZWriteControl;
        public MaterialProperty AlphaClip;
        public MaterialProperty ReceiveShadows;
        public MaterialProperty Cutoff;
        public MaterialProperty BaseMap;
        public MaterialProperty BaseColor;
        public MaterialProperty EmissionMap;
        public MaterialProperty EmissionColor;
        public MaterialProperty QueueOffset;

        public HumToonPropertiesContainer(PropertySetter matPropSetter)
        {
            _matPropSetter = matPropSetter;
        }

        public void Set()
        {
            _matPropSetter.Set(this);
        }
    }
}