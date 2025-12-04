<?php
include "conexion.php";

$data = json_decode(file_get_contents("php://input"));

$id_activo = $data->id_activo;
$nuevo = $data->id_usuario_destino;

$sql = "UPDATE activo SET propietario = ? WHERE id_activo = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ii", $nuevo, $id_activo);
$stmt->execute();

echo json_encode(["status" => "ok"]);
?>
