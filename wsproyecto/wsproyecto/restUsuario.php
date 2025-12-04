<?php
include "config.php";
include "utils.php";

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");
header("Content-Type: application/json; charset=UTF-8");

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

$dbConn = connect($db);

// ======================================================
//   FUNCIÓN PARA LEER JSON SI VIENE POR RAW BODY
// ======================================================
$raw = file_get_contents("php://input");
$input = json_decode($raw, true);

if (!$input) {  
    $input = $_POST;   // si no es JSON, usa form-data
}


// ======================================================
//   GET - OBTENER USUARIO(S)
// ======================================================
if ($_SERVER['REQUEST_METHOD'] == 'GET') {

    if (isset($_GET['id_usuario'])) {
        $sql = $dbConn->prepare("SELECT * FROM usuario WHERE id_usuario = :id_usuario");
        $sql->bindValue(':id_usuario', $_GET['id_usuario']);
        $sql->execute();

        $resp = $sql->fetch(PDO::FETCH_ASSOC);
        echo json_encode($resp);
        exit();
    }

    $sql = $dbConn->prepare("SELECT * FROM usuario");
    $sql->execute();
    echo json_encode($sql->fetchAll(PDO::FETCH_ASSOC));
    exit();
}

// ======================================================
//   POST - LOGIN
//   URL: restUsuario.php?login=1
//   BODY: { "correo":"...", "contrasena":"..." }
// ======================================================
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_GET['login'])) {

    if (!isset($input['correo']) || !isset($input['contrasena'])) {
        http_response_code(400);
        echo json_encode(["error" => "Correo y contraseña son requeridos"]);
        exit();
    }

    $sql = $dbConn->prepare("SELECT * FROM usuario WHERE correo = :correo AND contrasena = :contrasena");
    $sql->bindValue(':correo', $input['correo']);
    $sql->bindValue(':contrasena', $input['contrasena']);
    $sql->execute();

    $user = $sql->fetch(PDO::FETCH_ASSOC);

    if (!$user) {
        http_response_code(401); // No autorizado
        echo json_encode(["error" => "Correo o contraseña incorrectos"]);
        exit();
    }

    // Éxito
    http_response_code(200);
    echo json_encode($user);
    exit();
}


// ======================================================
//   POST - INSERTAR USUARIO
//   SOLO VALIDAR: CÉDULA y CORREO DUPLICADOS
// ======================================================
if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    // Validación de duplicado de cédula
    $sql = $dbConn->prepare("SELECT COUNT(*) AS total FROM usuario WHERE cedula = :cedula");
    $sql->bindValue(':cedula', $input['cedula']);
    $sql->execute();
    $row = $sql->fetch(PDO::FETCH_ASSOC);

    if ($row['total'] > 0) {
        http_response_code(409);
        echo json_encode(["error" => "La cédula ya está registrada"]);
        exit();
    }

    // Validación de correo duplicado
    $sql = $dbConn->prepare("SELECT COUNT(*) AS total FROM usuario WHERE correo = :correo");
    $sql->bindValue(':correo', $input['correo']);
    $sql->execute();
    $row = $sql->fetch(PDO::FETCH_ASSOC);

    if ($row['total'] > 0) {
        http_response_code(409);
        echo json_encode(["error" => "El correo ya está registrado"]);
        exit();
    }

    // INSERTAR USUARIO
    $sql = "INSERT INTO usuario (correo, contrasena, nombre, apellido, cedula, rol)
            VALUES (:correo, :contrasena, :nombre, :apellido, :cedula, :rol)";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    $input['id_usuario'] = $dbConn->lastInsertId();

    http_response_code(200);
    echo json_encode($input);
    exit();
}


// ======================================================
//   PUT - ACTUALIZAR USUARIO (VÍA QUERY STRING)
// ======================================================
if ($_SERVER['REQUEST_METHOD'] == 'PUT') {

    if (!isset($_GET['id_usuario'])) {
        http_response_code(400);
        echo json_encode(["error" => "Falta id_usuario"]);
        exit();
    }

    $id = $_GET['id_usuario'];

    // PARA PUT SE USA LA QUERY STRING (como lo envía tu MAUI)
    $input = $_GET;

    $fields = getParams($input);

    $sql = "UPDATE usuario SET $fields WHERE id_usuario = :id_usuario";

    $statement = $dbConn->prepare($sql);
    $statement->bindValue(':id_usuario', $id);
    bindAllValues($statement, $input);
    $statement->execute();

    http_response_code(200);
    echo json_encode(["status" => "actualizado"]);
    exit();
}



// ======================================================
//   SI NO COINCIDE NINGÚN MÉTODO
// ======================================================
http_response_code(400);
echo json_encode(["error" => "Solicitud inválida"]);

?>
