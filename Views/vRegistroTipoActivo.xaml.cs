using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;

namespace ProyectoFinal.Views
{
    public partial class vRegistroTipoActivo : ContentPage
    {
        private FileResult fotoTomada;

        public vRegistroTipoActivo()
        {
            InitializeComponent();
        }

        // =========================================================
        // TOMAR FOTO
        // =========================================================
        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return;

                fotoTomada = photo;

                using var stream = await photo.OpenReadAsync();
                var memory = new MemoryStream();
                await stream.CopyToAsync(memory);
                memory.Position = 0;

                imgPreview.Source = ImageSource.FromStream(() => new MemoryStream(memory.ToArray()));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo abrir la cámara:\n" + ex.Message, "OK");
            }
        }

        // =========================================================
        // GUARDAR TIPO ACTIVO
        // =========================================================
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    await DisplayAlert("Error", "Ingrese un nombre.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    await DisplayAlert("Error", "Ingrese una descripción.", "OK");
                    return;
                }

                string nombreImagen = "";

                // SUBIR IMAGEN SI EXISTE
                if (fotoTomada != null)
                {
                    nombreImagen = await SubirImagenApache(fotoTomada);

                    if (string.IsNullOrEmpty(nombreImagen))
                    {
                        bool continuar = await DisplayAlert("Advertencia",
                            "No se pudo subir la imagen. ¿Desea continuar sin imagen?",
                            "Sí", "No");

                        if (!continuar)
                            return;
                    }
                }

                // ENVIAR REGISTRO A PHP
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(txtNombre.Text), "nombre");
                form.Add(new StringContent(txtDescripcion.Text), "descripcion");
                form.Add(new StringContent(nombreImagen), "nombre_archivo");

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);

                HttpResponseMessage resp;
                try
                {
                    resp = await client.PostAsync("http://127.0.0.1/wsproyecto/restTipo_activo.php", form);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "No se pudo conectar al API:\n" + ex.Message, "OK");
                    return;
                }

                string respuesta = await resp.Content.ReadAsStringAsync();

                if (resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Tipo activo guardado correctamente.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "El servidor respondió con error:\n" + respuesta, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al guardar:\n" + ex.ToString(), "OK");
            }
        }

        // =========================================================
        // SUBIR IMAGEN A APACHE
        // =========================================================
        private async Task<string> SubirImagenApache(FileResult file)
        {
            try
            {
                using var stream = await file.OpenReadAsync();

                var form = new MultipartFormDataContent();
                var imgContent = new StreamContent(stream);

                // Detectar MIME
                string ext = Path.GetExtension(file.FileName)?.ToLower().Replace(".", "");
                string mime = ext switch
                {
                    "png" => "image/png",
                    "jpg" => "image/jpeg",
                    "jpeg" => "image/jpeg",
                    _ => "image/jpeg"
                };

                imgContent.Headers.ContentType = new MediaTypeHeaderValue(mime);
                form.Add(imgContent, "imagen", file.FileName);

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);

                HttpResponseMessage resp;

                try
                {
                    resp = await client.PostAsync("http://127.0.0.1/wsproyecto/upload_imagen.php", form);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Fallo de conexión al subir imagen:\n" + ex.Message, "OK");
                    return "";
                }

                string json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "Servidor de imágenes respondió con error:\n" + json, "OK");
                    return "";
                }

                try
                {
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("nombre_archivo", out JsonElement archivo))
                        return archivo.GetString() ?? "";

                    return "";
                }
                catch
                {
                    await DisplayAlert("Error", "La respuesta del servidor no es válida:\n" + json, "OK");
                    return "";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo procesar la imagen:\n" + ex.Message, "OK");
                return "";
            }
        }
    }
}
