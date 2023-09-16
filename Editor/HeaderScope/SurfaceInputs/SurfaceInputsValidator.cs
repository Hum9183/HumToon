using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HumToon.Editor
{
    public class SurfaceInputsValidator : IHeaderScopeValidator
    {
        private static readonly SurfaceInputsPropertyContainer P = new SurfaceInputsPropertyContainer(null);
        private static readonly int IDBumpMap = Shader.PropertyToID($"{nameof(P.BumpMap).Prefix()}");

        public void Validate(Material material)
        {
            SetKeywords(material);
        }

        private static void SetKeywords(Material material)
        {
            bool existsNormalMap = material.GetTexture(IDBumpMap) is not null;
            CoreUtils.SetKeyword(material, ShaderKeywordStrings._NORMALMAP, existsNormalMap);
        }
    }
}
