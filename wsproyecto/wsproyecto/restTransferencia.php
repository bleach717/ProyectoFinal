<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

/*
  ============================================
                MÉTODO GET
  ============================================
*/
if ($_SERVER['REQUEST_METHOD'] == 'GET') {

    // Consultar una transferencia específica
    if (isset($_GET['id_transferencia'])) {

        $sql = $dbConn->prepare("SELECT * FROM transferencia WHERE id_transferencia = :id_transferencia");
        $sql->bindValue(':id_transferencia', $_GET['id_transferencia']);
        $sql->execute();

        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    }

    // Listar todas las transferencias
    else {
        $sql = $dbConn->prepare("SELECT * FROM transferencia");
        $sql->execute();

        $sql->setFetchMode(PDO::FETCH_ASSOC);
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll());
        exit();
    }
}

/*
  ============================================
                MÉTODO DELETE
  ============================================
*/
if ($_SERVER['REQUEST_METHOD'] == 'DELETE') {

    $id_transferencia = $_GET['id_transferencia'];

    $statement = $dbConn->prepare("DELETE FROM transferencia WHERE id_transferencia = :id_transferencia");
    $statement->bindValue(':id_transferencia', $id_transferencia);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    exit();
}

/*
  ============================================
                MÉTODO POST
  ============================================
*/
if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    $input = $_POST;

    $sql = "INSERT INTO transferencia 
                (id_activo, id_usuario_origen, id_usuario_destino, fecha_transferencia)
            VALUES 
                (:id_activo, :id_usuario_origen, :id_usuario_destino, :fecha_transferencia)";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    $lastId = $dbConn->lastInsertId();

    if ($lastId) {
        $input['id_transferencia'] = $lastId;
        header("HTTP/1.1 200 OK");
        echo json_encode($input);
        exit();
    }
}

/*
  ============================================
                MÉTODO PUT
  ============================================
*/
if ($_SERVER['REQUEST_METHOD'] == 'PUT') {

    $input = $_GET;
    $id_transferencia = $input['id_transferencia'];

    // Genera "campo=:campo"
    $fields = getParams($input);

    $sql = "UPDATE transferencia SET $fields WHERE id_transferencia = '$id_transferencia'";

    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    header("HTTP/1.1 200 OK");
    exit();
}

?>
