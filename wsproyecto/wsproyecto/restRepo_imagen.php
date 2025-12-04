<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

// ===========================================
//                   GET
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'GET')
{
    if (isset($_GET['id_imagen']))
    {
        // Mostrar un registro por ID
        $sql = $dbConn->prepare("SELECT * FROM repo_imagen WHERE id_imagen = :id_imagen");
        $sql->bindValue(':id_imagen', $_GET['id_imagen']);
        $sql->execute();

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    }
    else
    {
        // Mostrar todos los registros
        $sql = $dbConn->prepare("SELECT * FROM repo_imagen");
        $sql->execute();
        $sql->setFetchMode(PDO::FETCH_ASSOC);

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll());
        exit();
    }
}

// ===========================================
//                 DELETE
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'DELETE')
{
    $id = $_GET['id_imagen'];

    $statement = $dbConn->prepare("DELETE FROM repo_imagen WHERE id_imagen = :id_imagen");
    $statement->bindValue(':id_imagen', $id);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    exit();
}

// ===========================================
//                  POST
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
    $input = $_POST;

    $sql = "INSERT INTO repo_imagen (ruta)
            VALUES (:ruta)";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    $newId = $dbConn->lastInsertId();

    if ($newId)
    {
        $input['id_imagen'] = $newId;
        header("HTTP/1.1 200 OK");
        echo json_encode($input);
        exit();
    }
}

// ===========================================
//                   PUT
// ===========================================
if ($_SERVER['REQUEST_METHOD'] == 'PUT')
{
    $input = $_GET;
    $id = $input['id_imagen'];

    $fields = getParams($input);

    $sql = "
        UPDATE repo_imagen
        SET $fields
        WHERE id_imagen = '$id'
    ";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    exit();
}

?>
