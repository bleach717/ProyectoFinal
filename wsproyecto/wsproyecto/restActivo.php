<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

// ===========================================
//                    GET
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'GET')
{
    if (isset($_GET['id_activo']))
    {
        // Mostrar un activo
        $sql = $dbConn->prepare("SELECT * FROM activo WHERE id_activo = :id_activo");
        $sql->bindValue(':id_activo', $_GET['id_activo']);
        $sql->execute();

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    }
    else
    {
        // Mostrar todos los activos
        $sql = $dbConn->prepare("SELECT * FROM activo");
        $sql->execute();
        $sql->setFetchMode(PDO::FETCH_ASSOC);

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll());
        exit();
    }
}

// ===========================================
//                   DELETE
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'DELETE')
{
    $id = $_GET['id_activo'];

    $statement = $dbConn->prepare("DELETE FROM activo WHERE id_activo = :id_activo");
    $statement->bindValue(':id_activo', $id);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    exit();
}

// ===========================================
//                    POST
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
    // Validar campos obligatorios
    if (!isset($_POST['id_tipo']) || 
        !isset($_POST['detalle']) ||
        !isset($_POST['estado']) ||
        !isset($_POST['costo']) ||
        !isset($_POST['ubicacion']) ||
        !isset($_POST['disponible']) ||
        !isset($_POST['propietario']))
    {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(["error" => "Faltan parámetros"]);
        exit();
    }

    // Asignar fecha actual
    $_POST['fecha_registro'] = date("Y-m-d");

    $sql = "INSERT INTO activo 
        (id_tipo, detalle, Marca, Modelo, Serie, Anio, estado, costo, ubicacion, disponible, fecha_registro, propietario)
        VALUES (:id_tipo, :detalle, :Marca, :Modelo, :Serie, :Anio, :estado, :costo, :ubicacion, :disponible, :fecha_registro, :propietario)";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $_POST);
    $statement->execute();

    $newId = $dbConn->lastInsertId();

    if ($newId)
    {
        $_POST['id_activo'] = $newId;

        header("HTTP/1.1 200 OK");
        echo json_encode($_POST);
        exit();
    }

    header("HTTP/1.1 500 Internal Server Error");
    echo json_encode(["error" => "No se pudo crear el activo"]);
    exit();
}

// ===========================================
//                    PUT
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'PUT')
{
    $input = $_GET;
    $id = $input['id_activo'];

    $fields = getParams($input);

    $sql = "
        UPDATE activo
        SET $fields
        WHERE id_activo = '$id'
    ";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    exit();
}

?>
