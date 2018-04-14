using UnityEngine;
using System.Collections;

namespace TMPro.Examples
{
    public class TextFade : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text[] m_TextComponent;

        public float FadeSpeed = 1.0F;
        public int RolloverCharacterSpread = 10;


        void Start()
        {
            for (int i = 0; i < m_TextComponent.Length; i++)
                StartCoroutine(AnimateVertexColors(m_TextComponent[i]));
        }


        /// <summary>
        /// Method to animate vertex colors of a TMP Text object.
        /// </summary>
        /// <returns></returns>
        IEnumerator AnimateVertexColors(TMP_Text textComponent)
        {
            // Need to force the text object to be generated so we have valid data to work with right from the start.
            textComponent.ForceMeshUpdate();


            TMP_TextInfo textInfo = textComponent.textInfo;
            Color32[] newVertexColors;

            int currentCharacter = 0;
            int startingCharacterRange = currentCharacter;
            bool isRangeMax = false;

            while (!isRangeMax)
            {
                int characterCount = textInfo.characterCount;

                // Spread should not exceed the number of characters.
                byte fadeSteps = (byte)Mathf.Max(1, 255 / RolloverCharacterSpread);


                for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
                {
                    // Skip characters that are not visible
                    if (!textInfo.characterInfo[i].isVisible) continue;

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Get the current character's alpha value.
                    byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a - fadeSteps, 0, 255);

                    // Set new alpha values.
                    newVertexColors[vertexIndex + 0].a = alpha;
                    newVertexColors[vertexIndex + 1].a = alpha;
                    newVertexColors[vertexIndex + 2].a = alpha;
                    newVertexColors[vertexIndex + 3].a = alpha;

                    if (alpha == 0)
                    {
                        startingCharacterRange += 1;

                        if (startingCharacterRange == characterCount)
                        {
                            // Update mesh vertex data one last time.
                            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                            yield return new WaitForSeconds(1.0f);

                            // Reset the text object back to original state.
                            textComponent.ForceMeshUpdate();

                            yield return new WaitForSeconds(1.0f);

                            // Reset our counters.
                            currentCharacter = 0;
                            startingCharacterRange = 0;
                            //isRangeMax = true; // Would end the coroutine.
                        }
                    }
                }

                // Upload the changed vertex colors to the Mesh.
                textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                if (currentCharacter + 1 < characterCount) currentCharacter += 1;

                yield return new WaitForSeconds(FadeSpeed * 0.01f);
            }
        }
    }
}