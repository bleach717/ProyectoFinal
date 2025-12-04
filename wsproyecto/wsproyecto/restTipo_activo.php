<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

// ===========================================
//                   GET
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'GET')
{
    // Obtener un tipo específico
    if (isset($_GET['id_tipo'])) {

        $sql = $dbConn->prepare("SELECT * 
                                 FROM tipo_activo 
                                 WHERE id_tipo = :id_tipo");

        $sql->bindValue(':id_tipo', $_GET['id_tipo']);
        $sql->execute();

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    } 
    else 
    {
        // Obtener todos los tipos
        $sql = $dbConn->prepare("SELECT * FROM tipo_activo");
        $sql->execute();

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll(PDO::FETCH_ASSOC));
        exit();
    }
}

// ===========================================
//                  POST
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    $data = $_POST;

    if (empty($data)) {
        $data = json_decode(file_get_contents('php://input'), true);
    }

    if (!isset($data['nombre'])) {
        echo json_encode(["error" => "nombre es requerido"]);
        exit();
    }

    // Si no llega imagen, se guarda null
    $ruta_imagen = $data['nombre_archivo'] ?? null;

    $sql = "
        INSERT INTO tipo_activo (nombre, descripcion, ruta_imagen)
        VALUES (:nombre, :descripcion, :ruta_imagen)
    ";

    $statement = $dbConn->prepare($sql);
    $statement->bindValue(':nombre', $data['nombre']);
    $statement->bindValue(':descripcion', $data['descripcion'] ?? null);
    $statement->bindValue(':ruta_imagen', $ruta_imagen);
    $statement->execute();

    $data['id_tipo'] = $dbConn->lastInsertId();

    echo json_encode($data);
    exit();
}

// ===========================================
//                   PUT
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'PUT') {

    parse_str(file_get_contents("php://input"), $input);

    if (!isset($input['id_tipo'])) {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(["error" => "id_tipo es requerido"]);
        exit();
    }

    $id = $input['id_tipo'];

    $fields = getParams($input);

    $sql = "UPDATE tipo_activo SET $fields WHERE id_tipo = :id_tipo";

    $statement = $dbConn->prepare($sql);
    $statement->bindValue(':id_tipo', $id);

    bindAllValues($statement, $input);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    echo json_encode(["message" => "Actualizado correctamente"]);
    exit();
}


// ===========================================
//                 DELETE
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'DELETE') {

    if (!isset($_GET['id_tipo'])) {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(["error" => "id_tipo es requerido"]);
        exit();
    }

    $id = $_GET['id_tipo'];

    $statement = $dbConn->prepare("
        DELETE FROM tipo_activo 
        WHERE id_tipo = :id_tipo
    ");
    $statement->bindValue(':id_tipo', $id);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    echo json_encode(["message" => "Eliminado"]);
    exit();
}

?>