<?php

header("Content-Type: application/json; charset=UTF-8");

// Carpeta donde se guardarán las imágenes
$carpeta = "imagenes_tipo/";

// Crear carpeta si no existe
if (!file_exists($carpeta)) {
    mkdir($carpeta, 0777, true);
}

// Validar que se reciba el archivo
if (!isset($_FILES['imagen'])) {
    echo json_encode([
        "error" => true,
        "mensaje" => "No se envió ninguna imagen"
    ]);
    exit();
}

$archivo = $_FILES['imagen'];

// Validar errores de subida
if ($archivo['error'] !== UPLOAD_ERR_OK) {
    echo json_encode([
        "error" => true,
        "mensaje" => "Error al subir la imagen"
    ]);
    exit();
}

// Extensiones permitidas
$extensiones_permitidas = ["jpg", "jpeg", "png"];
$extension = strtolower(pathinfo($archivo['name'], PATHINFO_EXTENSION));

if (!in_array($extension, $extensiones_permitidas)) {
    echo json_encode([
        "error" => true,
        "mensaje" => "Tipo de archivo no permitido"
    ]);
    exit();
}

// Generar nombre único
$nombre = uniqid("img_", true) . "." . $extension;
$ruta = $carpeta . $nombre;

// Mover archivo
if (move_uploaded_file($archivo['tmp_name'], $ruta)) {

    echo json_encode([
        "error" => false,
        "mensaje" => "Imagen subida correctamente",
        "nombre_archivo" => $nombre,
        "ruta" => $ruta
    ]);
    exit();
}

echo json_encode([
    "error" => true,
    "mensaje" => "No se pudo guardar la imagen"
]);
exit();
