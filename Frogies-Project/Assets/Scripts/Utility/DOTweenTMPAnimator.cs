using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class DOTweenTMPAnimator
    {
        private TMP_Text _text;
        

        public DOTweenTMPAnimator(TMP_Text text)
        {
            _text = text;
        }

        public Tween DOText(string to, float duration)
        {
            _text.DOKill();
            _text.text = to;
            _text.ForceMeshUpdate();
            var textInfo = _text.textInfo;
            
            for (var i = 0; i < textInfo.characterInfo.Length; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                var cols = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                var color = cols[vertexIndex];
                color.a = 0;
                
                cols[vertexIndex] = color;
                cols[vertexIndex + 1] = color;
                cols[vertexIndex + 2] = color;
                cols[vertexIndex + 3] = color;
            }
            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            
            for (int i = 0; i < textInfo.linkInfo.Length; i++)
            {
                var linkInfo = textInfo.linkInfo[i];
                var id = linkInfo.GetLinkID();
                var linkType = GetLinkAnimationType(id);
                InteractiveLink(linkType, linkInfo.linkTextfirstCharacterIndex, linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1)
                    .SetLoops(-1)
                    .SetLink(_text.gameObject, LinkBehaviour.KillOnDisable);
            }
            
            int index = 0;
            int prevIndex = 0; // dotween can skip some values, so we need to keep track of previous index
            Debug.Log("Start");
            return DOTween.To(() => index, (x) => index = x, to.Length - 1, duration).OnUpdate(() =>
            {
                if(index == prevIndex)
                    return;

                for (int i = prevIndex; i <= index; i++)
                {
                    Debug.Log(i);
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    var cols = textInfo.meshInfo[materialIndex].colors32;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    var color = cols[vertexIndex];
                    color.a = 255;
                
                    cols[vertexIndex] = color;
                    cols[vertexIndex + 1] = color;  
                    cols[vertexIndex + 2] = color;
                    cols[vertexIndex + 3] = color;
                }

                prevIndex = index; 
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            });
        }

        public Tween DoShake(int startIndex, int endIndex, float strength, float vibration, float duration = 2)
        {
            var textInfo = _text.textInfo;
            float shakeTime = 0;
            Vector3[] origins = new Vector3[ (endIndex - startIndex + 1) * 4];
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                var verts = textInfo.meshInfo[materialIndex].vertices;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                int charIndex = i - startIndex;
                origins[charIndex + 0] = verts[vertexIndex + 0];
                origins[charIndex + 1] = verts[vertexIndex + 1];
                origins[charIndex + 2] = verts[vertexIndex + 2];
                origins[charIndex + 3] = verts[vertexIndex + 3];
            }
            
            var rnd = new System.Random();
            return DOTween.To(()=> shakeTime, x=> shakeTime = x, 1f, duration)
                .OnUpdate(() =>
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    /*shakeVector.x = Mathf.Cos(shakeTime + (float) rnd.NextDouble() * vibration) * strength;
                    shakeVector.y = Mathf.Sin(shakeTime + (float) rnd.NextDouble() * vibration) * strength;*/
                    Vector3 shakeVector = Vector3.zero;
                    /*shakeVector.x = (float) rnd.NextDouble() * strength * Mathf.Sin(shakeTime + i * vibration);
                    shakeVector.y = (float) rnd.NextDouble() * strength * Mathf.Sin(shakeTime * vibration + i * vibration);*/
                    
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    var verts = textInfo.meshInfo[materialIndex].vertices;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    int charIndex = i - startIndex;
                    verts[vertexIndex + 0] = origins[charIndex + 0] + shakeVector;
                    verts[vertexIndex + 1] = origins[charIndex + 1] + shakeVector;
                    verts[vertexIndex + 2] = origins[charIndex + 2] + shakeVector;
                    verts[vertexIndex + 3] = origins[charIndex + 3] + shakeVector;
                }
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            });
        }
        
        public Tween DoWave(int startIndex, int endIndex, float duration = 2)
        {
            var textInfo = _text.textInfo;
            float waveTime = 0;
            return DOTween.To(()=> waveTime, x=> waveTime = x, 1f, duration)
                .OnUpdate(() =>
                {
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                        var verts = textInfo.meshInfo[materialIndex].vertices;
                        int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                        float waveFactor = Mathf.Sin(waveTime + i * 0.2f);
                        verts[vertexIndex + 0] += new Vector3(0, waveFactor, 0);
                        verts[vertexIndex + 1] += new Vector3(0, waveFactor, 0);
                        verts[vertexIndex + 2] += new Vector3(0, waveFactor, 0);
                        verts[vertexIndex + 3] += new Vector3(0, waveFactor, 0);
                    }
                    _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                });
        }
        
        private Tween InteractiveLink(AnimationType link, int startIndex, int endIndex)
        {
            return link switch
            {
                AnimationType.Shake => DoShake(startIndex, endIndex, .2f, 2f),
                AnimationType.Wave => DoWave(startIndex, endIndex),
                _ => null
            };
        }

        private AnimationType GetLinkAnimationType(string linkId)
        {
            return linkId switch
            {
                "shake" => AnimationType.Shake,
                "wave" => AnimationType.Wave,
                _ => AnimationType.Unknown,
            };
        }

        public enum AnimationType
        {
            Unknown,
            Shake,
            Wave
        }
    }
}