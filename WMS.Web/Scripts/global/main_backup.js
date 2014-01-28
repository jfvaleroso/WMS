$(function () {


    var item = $("#Item").val();
    var home = $("#Home").val();
    var create = $("#New").val();
    var manage = $("#Manage").val();
    var remove = $("#Delete").val();
    var id = $("#Id").val();
    var securedId = $("#SecuredId").val();
    var postData = $("#PostData").val();
    var checkAvailability = $("#CheckAvailability").val();


    $(".combobox").select2({
        placeholder: "Select one",
        allowClear: true
    });

    $('.container').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    })



    $("#btnCancel").click(function (event) {
        event.preventDefault();
        $('#modalWaiting').modal('show');
        setTimeout(function () {
            $('#modalWaiting').on('hidden.bs.modal', function () {
                window.history.back();
            }).modal('hide')
        }, 500);
    });


    $("#btnBackToList").click(function (event) {
        event.preventDefault();
        $('#modalWaiting').modal('show');
        setTimeout(function () {
            $('#modalWaiting').on('hidden.bs.modal', function () {
                window.location = home;
            }).modal('hide')
        }, 500);
    });

    $("#btnModify").click(function (event) {
        event.preventDefault();
        $('#modalWaiting').modal('show');
        setTimeout(function () {
            $('#modalWaiting').on('hidden.bs.modal', function () {
                window.location = manage + "/" + securedId;
            }).modal('hide')
        }, 500);
    });

    //$("#home").find("#btnSave").click(function (event) {
    $("#btnSave1").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: create,
            type: "post",
            data: $('form').serialize(),
            beforeSend: function () {
                $('form').validate().form();
            },
            complete: function (data) {
            },
            error: function (data) {

            },
            success: function (data) {
                //saved
                if (data.code == '000') {
                    $('#modalSaving').modal('show');
                    setTimeout(function () {
                        $('#modalSaving').on('hidden.bs.modal', function () {
                            window.location = item + "/" + data.result + "/" + data.code;
                        }).modal('hide')
                    }, 500);
                }
                //invalid
                else if (data.code == "007") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Required field!');
                }
                //existed
                else if (data.code == "003") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Item already exists!');
                }
                //error
                else if (data.code == "005") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Error has occured!' + data.message);
                }

            }
        });
    });


    $("#btnSaveChanges").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: manage,
            type: "post",
            data: $("form").serialize(),
            beforeSend: function () {
                $('form').validate().form();
            },
            complete: function (data) {
            },
            error: function (data) {

            },
            success: function (data) {
                //modified
                if (data.code == '001') {
                    $('#modalSaving').modal('show');
                    setTimeout(function () {
                        $('#modalSaving').on('hidden.bs.modal', function () {
                            window.location = item + "/" + data.result + "/" + data.code; ;
                        }).modal('hide')
                    }, 500);
                }
                //invalid
                else if (data.code == "007") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Required field!');
                }
                //existed
                else if (data.code == "003") {
                    $('#alert').removeClass('hide').removeClass('alert-danger ').addClass('alert-warning');
                    $('#notification').empty().append('No changes have been made!');
                }
                //error
                else if (data.code == "005") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Error has occured!');
                }
            }
        });
    });


    $("#btnConfirmation").click(function (event) {
        event.preventDefault();
        $('#modalConfirmation').modal('show');
    });

    $("#btnDelete").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: remove,
            type: "post",
            data: $("form").serialize(),
            complete: function (data) {
            },
            error: function (data) {
            },
            success: function (data) {
                if (data.code == '002') {
                    $('#modalConfirmation').modal('hide');
                    $('#modalDeleting').modal('show');
                    setTimeout(function () {
                        $('#modalDeleting').on('hidden.bs.modal', function () {
                            window.location = home;
                        }).modal('hide')
                    }, 500);
                }
            }
        });
    });


    $("#btnNew").click(function (event) {
        event.preventDefault();
        $('#modalWaiting').modal('show');
        setTimeout(function () {
            $('#modalWaiting').on('hidden.bs.modal', function () {
                window.location = create;
            }).modal('hide')
        }, 500);
    });







    $("#btntest").click(function (event) {
        event.preventDefault();
        $('#modaltest').modal('show');
    });
    $("#Code").focusout(function () {
        event.preventDefault();
        $.ajax({
            url: checkAvailability,
            type: "post",
            data: { param: $("#Code") .val()},
            complete: function (data) {
            },
            error: function (data) {
                alert(data.code);
            },
            success: function (data) {
                if (data.code == '008') {
                    $('#status').empty().append('Code is available!');
                    $('#status').addClass('alert').removeClass('alert-warning').addClass('alert-success').addClass('fade-in');
                }
                else if (data.code == '003') {
                    $('#status').empty().append('Code already exists!');
                    $('#status').addClass('alert').removeClass('alert-success').addClass('alert-warning').addClass('fade-in');
                }
                else {
                    $('#status').empty().append('Verify if Code exists.!');
                    $('#status').removeClass('alert').removeClass('alert-warning').removeClass('alert-success').removeClass('fade-in');
                }
            }
            });
    });
    $("#btnSaveTest").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: create,
            type: "post",
            data: $('form').serialize(),
            beforeSend: function () {
                $('form').validate().form();
            },
            complete: function (data) {
            },
            error: function (data) {

            },
            success: function (data) {
                //saved
                if (data.code == '000') {
                    $('#modal').modal('hide');
                    alert('done');
                    //$('#message').empty().append(data.message + data.content);
                    //$('#message').addClass('alert').addClass('alert-success').addClass('fade-in');
                    //$('#btnSearch').click();
                }
                    //invalid
                else if (data.code == "007") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Required field!');
                }
                    //existed
                else if (data.code == "003") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Item already exists!');
                }
                    //error
                else if (data.code == "005") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Error has occured!' + data.message);
                }

            }
        });
    });
    $("#btntestEdit").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: manage,
            type: "get",
            data: { id: 1},
            complete: function (data) {
            },
            error: function (data) {
                alert('errro')
            },
            success: function (data) {
                $('#modaltestEdit').modal('show');
                $('#modaltestEdit').replaceWith(data);

            }
        });
    });
    $("#showGame").click(function () {
       // var url = $("#gameModal").data("url");
        $('#gameModal').modal('show');
        //$.get(url, function (data) {
        //    $("#gameContainer").html(data);
        //    $("#gameModal").modal("show");
        //});
    });


   

























});