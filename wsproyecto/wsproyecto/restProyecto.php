<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

// =======================================================
//  VALIDAR TABLA (SEGURIDAD)
// =======================================================
$allowedTables = ['usuario','repo_imagen','tipo_activo','activo','transferencia'];

if (!isset($_GET['table']) || !in_array($_GET['table'], $allowedTables)) {
    header("HTTP/1.1 400 Bad Request");
    echo json_encode(["error" => "Tabla no permitida"]);
    exit();
}

$table = $_GET['table'];
$idField = "id_" . $table; // nombre del campo ID (ej: id_activo)


// =======================================================
//  GET
// =======================================================
if ($_SERVER['REQUEST_METHOD'] == 'GET') {

    if ($table == "activo") {

        if (isset($_GET['id'])) {
            $sql = $dbConn->prepare(
                "SELECT a.*, u.nombre, u.apellido 
                 FROM activo a
                 LEFT JOIN usuario u ON a.propietario = u.id_usuario
                 WHERE a.id_activo = :id"
            );
            $sql->bindValue(':id', $_GET['id']);
            $sql->execute();

            echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
            exit();
        } else {
            $sql = $dbConn->prepare(
                "SELECT a.*, u.nombre, u.apellido 
                 FROM activo a
                 LEFT JOIN usuario u ON a.propietario = u.id_usuario"
            );
            $sql->execute();

            echo json_encode($sql->fetchAll(PDO::FETCH_ASSOC));
            exit();
        }
    }

    // GET Genérico
    if (isset($_GET['id'])) {
        $sql = $dbConn->prepare("SELECT * FROM $table WHERE $idField = :id");
        $sql->bindValue(':id', $_GET['id']);
        $sql->execute();

        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    } else {
        $sql = $dbConn->prepare("SELECT * FROM $table");
        $sql->execute();

        echo json_encode($sql->fetchAll(PDO::FETCH_ASSOC));
        exit();
    }
}



// =======================================================
//  DELETE
// =======================================================
if ($_SERVER['REQUEST_METHOD'] == 'DELETE') {

    if (!isset($_GET['id'])) {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(["error" => "Falta parámetro id"]);
        exit();
    }

    // Verificar si el registro existe
    $check = $dbConn->prepare("SELECT COUNT(*) FROM $table WHERE $idField = :id");
    $check->bindValue(':id', $_GET['id']);
    $check->execute();

    if ($check->fetchColumn() == 0) {
        header("HTTP/1.1 404 Not Found");
        echo json_encode(["error" => "El registro no existe"]);
        exit();
    }

    // Ahora sí eliminar
    $sql = $dbConn->prepare("DELETE FROM $table WHERE $idField = :id LIMIT 1");
    $sql->bindValue(':id', $_GET['id']);
    $sql->execute();

    echo json_encode(["message" => "Registro eliminado correctamente"]);
    exit();
}



// =======================================================
//  POST (INSERTAR)
//  *** TRANSFERENCIA CON ACTUALIZACIÓN DE ACTIVO ***
// =======================================================
if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    $input = array_filter($_POST, fn($value) => $value !== "");

    // --- CASO ESPECIAL: TRANSFERENCIA ---
    if ($table == "transferencia") {

        if (!isset($input['id_activo'], $input['id_usuario_origen'], $input['id_usuario_destino'])) {
            header("HTTP/1.1 400 Bad Request");
            echo json_encode(["error" => "Parámetros incompletos para transferencia"]);
            exit();
        }

        try {
            $dbConn->beginTransaction();

            // 1) Insertar en transferencia
            $sql1 = $dbConn->prepare(
                "INSERT INTO transferencia 
                (id_activo, id_usuario_origen, id_usuario_destino, fecha_transferencia)
                VALUES (:id_activo, :id_usuario_origen, :id_usuario_destino, NOW())"
            );
            bindAllValues($sql1, $input);
            $sql1->execute();

            // 2) Actualizar propietario del activo
            $sql2 = $dbConn->prepare(
                "UPDATE activo SET propietario = :nuevo 
                 WHERE id_activo = :id_activo"
            );
            $sql2->bindValue(':nuevo', $input['id_usuario_destino']);
            $sql2->bindValue(':id_activo', $input['id_activo']);
            $sql2->execute();

            $dbConn->commit();

            echo json_encode([
                "status" => "ok",
                "message" => "Transferencia realizada y propietario actualizado"
            ]);
            exit();

        } catch (Exception $e) {

            $dbConn->rollBack();

            header("HTTP/1.1 500 Internal Server Error");
            echo json_encode([
                "error" => "Error al procesar la transferencia",
                "detalle" => $e->getMessage()
            ]);
            exit();
        }
    }

    // --- POST GENÉRICO ---
    $columns = implode(", ", array_keys($input));
    $values = ":" . implode(", :", array_keys($input));

    $sql = $dbConn->prepare("INSERT INTO $table ($columns) VALUES ($values)");
    bindAllValues($sql, $input);
    $sql->execute();

    $insertedId = $dbConn->lastInsertId();

    echo json_encode([
        "message" => "Registro insertado correctamente",
        "id" => $insertedId,
        "data" => $input
    ]);
    exit();
}



// =======================================================
//  PUT (ACTUALIZAR)
// =======================================================
if ($_SERVER['REQUEST_METHOD'] == 'PUT') {

    parse_str(file_get_contents("php://input"), $input);

    if (!isset($_GET['id'])) {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(["error" => "Falta parámetro id"]);
        exit();
    }

    $id = $_GET['id'];
    $input = array_filter($input, fn($value) => $value !== "");

    $fields = getParams($input);

    $sql = $dbConn->prepare("UPDATE $table SET $fields WHERE $idField = :id");

    bindAllValues($sql, $input);
    $sql->bindValue(':id', $id);
    $sql->execute();

    echo json_encode(["message" => "Registro actualizado correctamente"]);
    exit();
}



// =======================================================
//  MÉTODO NO VÁLIDO
// =======================================================
header("HTTP/1.1 400 Bad Request");
echo json_encode(["error" => "Método no permitido"]);
exit();

?>