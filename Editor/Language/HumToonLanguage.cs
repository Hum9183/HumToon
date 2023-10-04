using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hum.HumToon.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.Language
{
    // TODO: クラスが大きくなってきたため、移譲する
    public static class HumToonLanguage
    {
        private const Language DefaultLang = Language.English;
        private const string EditorUserSettingsConfigName = "HumToonLanguage";

        private static Language currentLanguage;

        private static GUIContent LanguageLabel =>
            EditorGUIUtility.TrTextContent(
                text: $"{Select(new string[] { "Language", "言語", "语言" })}");

        public static string Select(string[] texts)
        {
            string result = string.Empty;

            if (texts.TryGetValue((int)DefaultLang, out string defaultLangText))
            {
                if (string.IsNullOrEmpty(defaultLangText) is false)
                    result = defaultLangText;
            }

            if (texts.TryGetValue((int)currentLanguage, out string currentLangText))
            {
                if (string.IsNullOrEmpty(currentLangText) is false)
                    result = currentLangText;
            }

            return result;
        }

        public static string[] DisplayedOptions<T>()
            where T: Enum
        {
            // Ref: https://web.archive.org/web/20181119155348/http://www.distribucon.com/blog/GettingMembersOfAnEnumViaReflection.aspx
            var enumFields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

            var currentLangAttrs = new List<DisplayNameLanguageAttributeBase>();
            foreach (var field in enumFields)
            {
                var existingLangAttrs = Attribute.GetCustomAttributes(field).ToList().OfType<DisplayNameLanguageAttributeBase>();
                var missingLangAttrs = CreateMissingLanguageAttributes(field);
                var allLangAttrs = existingLangAttrs.Concat(missingLangAttrs);
                var currentLangAttr = SortByCurrentLang(allLangAttrs);
                currentLangAttrs.Add(currentLangAttr);
            }

            var displayedOptions = currentLangAttrs
                .OrderBy(x => x.Enum)
                .Select(x => x.DisplayName)
                .ToArray();

            return displayedOptions;
        }

        /// <summary>
        /// 付与されていない言語アトリビュートを生成する
        /// </summary>
        private static IEnumerable<DisplayNameLanguageAttributeBase> CreateMissingLanguageAttributes(FieldInfo field)
        {
            // NOTE:
            // アトリビュートが付与されていない場合は、その分インスタンスを生成する。
            // インスタンス生成時の引数(ディスプレイ名)はフィールド名(Enumの項目名)
            return HumToonUtils.GetSubclasses<DisplayNameLanguageAttributeBase>()
                .Where(x => field.IsDefined(x) is false)
                .Select(x => Activator.CreateInstance(x, field.Name) as DisplayNameLanguageAttributeBase);
        }

        /// <summary>
        /// 現在の言語でソートする
        /// </summary>
        private static DisplayNameLanguageAttributeBase SortByCurrentLang(IEnumerable<DisplayNameLanguageAttributeBase> langAttrs)
        {
            return langAttrs.FirstOrDefault(x => x.Enum == currentLanguage);
        }

        /// <summary>
        /// Draw language GUI
        /// </summary>
        public static void Draw()
        {
            int currentLang = GetFromEditorUserSettings();
            int newLang = DrawInternal(currentLang);
            SetEditorUserSettings(newLang);
            SetCurrentLanguage(newLang);
        }

        private static int GetFromEditorUserSettings()
        {
            string langStr = EditorUserSettings.GetConfigValue(EditorUserSettingsConfigName); // e.g. "0", "1", "2"
            langStr ??= ((int)DefaultLang).ToString();

            bool success = int.TryParse(langStr, out int langInt);
            return success ? langInt : (int)DefaultLang;
        }

        private static int DrawInternal(int lang)
        {
            // TODO: Undo
            int newValue = EditorGUILayout.Popup(LanguageLabel, lang, DisplayedOptions<Language>());
            return newValue;
        }

        private static void SetEditorUserSettings(int lang)
        {
            EditorUserSettings.SetConfigValue(EditorUserSettingsConfigName, lang.ToString());
        }

        private static void SetCurrentLanguage(int newLang)
        {
            currentLanguage = (Language)newLang;
        }
    }
}
