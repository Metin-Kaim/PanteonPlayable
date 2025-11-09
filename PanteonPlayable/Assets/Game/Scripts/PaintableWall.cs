using UnityEngine;
using System.Collections.Generic;
using Assets.Game.Scripts.Signals;

public class PaintableWall : MonoBehaviour
{
    [SerializeField] private Renderer wallRenderer;
    [SerializeField] private int maskResolution = 256;
    [SerializeField] private float brushRadius = 0.02f;
    [SerializeField] private float minBrushRadius = 0.02f;
    [SerializeField] private float maxBrushRadius = 0.1f;
    [SerializeField] private LayerMask paintLayer;

    private Color paintColor;
    private Texture2D maskTexture;
    private Color[] maskPixels;
    private HashSet<int> paintedPixels = new HashSet<int>();
    private Camera mainCam;
    private MaterialPropertyBlock mpb;

    private void OnEnable()
    {
        PaintSignals.Instance.onSetPaintColor += SetPaintColor;
        PlayableSignals.Instance.onGoToStore += OnGoToStore;
    }

    private void OnGoToStore()
    {
        enabled = false;
    }

    private void OnDisable()
    {
        PaintSignals.Instance.onSetPaintColor -= SetPaintColor;
        PlayableSignals.Instance.onGoToStore -= OnGoToStore;
    }

    void Start()
    {
        mainCam = Camera.main;
        mpb = new MaterialPropertyBlock();

        // WebGL ve Playworks uyumlu format
        maskTexture = new Texture2D(maskResolution, maskResolution, TextureFormat.RGBA32, false);
        maskTexture.filterMode = FilterMode.Point; // <- EKLENDİ
        maskPixels = new Color[maskResolution * maskResolution];


        // Başlangıçta tamamen beyaz (duvar boyasız = baseColor)
        for (int i = 0; i < maskPixels.Length; i++)
            maskPixels[i] = new Color(0, 0, 0, 0); // alpha 0 -> boyasız

        maskTexture.SetPixels(maskPixels);
        maskTexture.Apply();

        mpb.SetTexture("_MaskTex", maskTexture);
        wallRenderer.SetPropertyBlock(mpb);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50, paintLayer))
                PaintAt(hit.textureCoord);
        }
    }

    void PaintAt(Vector2 uv)
    {
        int x = (int)(uv.x * maskResolution);
        int y = (int)(uv.y * maskResolution);
        int radius = Mathf.RoundToInt(brushRadius * maskResolution);

        for (int i = -radius; i < radius; i++)
        {
            for (int j = -radius; j < radius; j++)
            {
                int px = x + i;
                int py = y + j;
                if (px < 0 || px >= maskResolution || py < 0 || py >= maskResolution)
                    continue;

                float dist = Mathf.Sqrt(i * i + j * j);
                if (dist < radius)
                {
                    int index = py * maskResolution + px;

                    Color newColor = paintColor;
                    newColor.a = 1f;

                    maskPixels[index] = newColor;
                    paintedPixels.Add(index);
                }
            }
        }

        maskTexture.SetPixels(maskPixels);
        maskTexture.Apply();

        mpb.SetTexture("_MaskTex", maskTexture);
        wallRenderer.SetPropertyBlock(mpb);

        float percent = (float)paintedPixels.Count / maskPixels.Length * 100f;
        PaintSignals.Instance.onSetPaintPercent.Invoke($"{(int)percent}%");

        if (percent >= 100)
        {
            PlayableSignals.Instance.onGoToStore.Invoke();
        }
    }


    private void SetPaintColor(Color newColor)
    {
        paintColor = newColor;
        mpb.SetColor("_PaintColor", paintColor);
        wallRenderer.SetPropertyBlock(mpb);
    }

    public void SetBrushSize(float size)
    {
        brushRadius = Mathf.Lerp(minBrushRadius, maxBrushRadius, Mathf.Clamp01(size));
    }
}
