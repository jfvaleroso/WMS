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

    var newTitle = $("#NewTitle").val();
    var editTitle = $("#EditTitle").val();

    var documentMapping = $("#DocumentMapping").val();

    $(".combobox").select2({
        placeholder: "Select one",
        allowClear: true
    });


    $('#DataTable, #notification-list,#data-list').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    })



    //activate jTable
    //Re-load records when user click 'load records' button.
    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searhString = $('#searchString').val();
        $('#DataTable').jtable('load', {
            searchString: searhString
        });
    });
 
    //include partial view within the global modal
    function getRequest(url, title) {
        $.ajax({
            url: url,
            context: document.body,
            cache: false,
            success: function (data) {
                $("#transaction").removeClass("hide");
                $('#mainModal .modal-body').html(data);
                $(this).addClass("done");
                $('#mainModal .modal-header .modal-title').empty().append(title);
                $('#mainModal').modal('show');
                //activate select 2
                $(".combobox").select2({
                    placeholder: "Select one",
                    allowClear: true
                });
            },
            error: function (err) {
                alert(err);
            }
        });
    }
    //call add button and populate to partial view
    //connect to partial view
    $('#btnNew').click(function (event) {
        event.preventDefault();
        var url = create;
        getRequest(url, newTitle);
        return false;
    });
    // post: save
    $("#btnSave").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: create,
            type: "post",
            cache: false,
            data: $('form').serialize(),
            beforeSend: function () {
                $('#transaction form').validate().form();
            },
            complete: function (data) {
            },
            error: function (data) {
                alert('error');
            },
            success: function (data) {
                //saved
                if (data.code == '000') {
                    $('#mainModal').modal('hide');
                    $('#DataTable').jtable('load');
                    //add notification
                    $('#panel-status').removeClass().addClass('panel fade in panel-success');
                    $('#general-status').empty().append(data.message + data.content);
                    $('#alert').addClass('hide');
                }
                    //invalid
                else if (data.code == "007") {
                    $('#alert').removeClass('hide');
                  //  $('#notification').empty().append('Required field!');
                }
                    //existed
                else if (data.code == "003") {
                    $('#alert').removeClass('hide');
                    $('#notification').addClass('validation-summary-errors');
                    $('#notification').empty().append('<ul><li>Code or Item already exists!</li></ul>');
                    $('div.validation-summary-errors').addClass('hide');
                }
                    //error
                else if (data.code == "005") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Error has occured!' + data.message);
                    alert('test');
                }
              
            }
        });
    });
    // post: edit
    $(document).on("click","#btnSaveChanges", function(event)   {  
        event.preventDefault();
        $.ajax({
            url: manage,
            type: "post",
            data: $("form").serialize(),
            cache: false,
            beforeSend: function () {
                $('#transaction form').validate().form();
            },
            complete: function (data) {
            },
            error: function (data) {

            },
            success: function (data) {
                //modified
                if (data.code == '001') {
                    $('#mainModal').modal('hide');
                    $('#DataTable').jtable('load');
                    //add notification
                    $('#panel-status').removeClass().addClass('panel fade in panel-warning');
                    $('#general-status').empty().append(data.message + data.content);
                }
                    //invalid
                else if (data.code == "007") {
                    $('#alert').removeClass('hide');
                  //  $('#notification').empty().append('Required field!');
                }
                    //existed
                else if (data.code == "003") {
                    $('#alert').removeClass('hide');
                    $('#notification').addClass('validation-summary-errors');
                    $('#notification').empty().append('<ul><li>Item already exists!</li></ul>');
                    $('div.validation-summary-errors').addClass('hide');
                }
                    //error
                else if (data.code == "005") {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Error has occured!');
                }
            }
        });
    });
    //code validation
    function CheckCode(item) {
        $.ajax({
            url: checkAvailability,
            type: "post",
            data: { param: item },
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

    }


    $("#Code").focusout(function () {
        event.preventDefault();
        CheckCode($("#Code").val());
    });

    //$("#Process #Code").focusout(function () {
    //    event.preventDefault();
    //    var process = $("#Code").val();
    //    var code = "";
    //    if (process != '') {
    //        code = $("#SystemCode").val() + '-' + process;
    //    }
    //    CheckCode(code);
    //});
    //$("#SubProcess #Code").focusout(function () {
    //    event.preventDefault();
    //    var subProcess = $("#Code").val();
    //    var code = "";
    //    if (subProcess != '') {
    //        code = subProcess;
    //    }
    //    CheckCode(code);
    //});
    //$("#Classification #Code").focusout(function () {
    //    event.preventDefault();
    //    var classification = $("#Code").val();
    //    var code = "";
    //    if (classification != '') {
    //        code = classification;
    //    }
    //    CheckCode(code);
    //});






    $("#btnCancel, #btnBack").click(function (event) {
        event.preventDefault();
        window.history.back();
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
    //$("#btnModify").click(function (event) {
    //    event.preventDefault();
    //    $('#modalWaiting').modal('show');
    //    setTimeout(function () {
    //        $('#modalWaiting').on('hidden.bs.modal', function () {
    //            window.location = manage + "/" + securedId;
    //        }).modal('hide')
    //    }, 500);
    //});
    //$("#home").find("#btnSave").click(function (event) {
  
  
    $("#btnConfirmation").click(function (event) {
        event.preventDefault();
        $('#modalConfirmation').modal('show');
    });
    
    $("#btnDelete").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: remove,
            type: "post",
            data: { id: $('#Id').val() },
            complete: function (data) {
            },
            error: function (data) {
                alert('error');
            },
            success: function (data) {
                //deleted
                if (data.code == '002') {
                    $('#mainModal').modal('hide');
                    $('#DataTable').jtable('load');
                    //add notification
                    $('#panel-status').removeClass().addClass('panel fade in panel-success');
                    $('#general-status').empty().append(data.message + $('#Item').val());
                    //hide modal
                    $('#modalConfirmation').modal('hide');

                }
                    //error
                else  {
                    $('#alert').removeClass('hide');
                    $('#notification').empty().append('Error has occured!' + data.message);
                }
            }
        });
    });
    //$("#btnNew").click(function (event) {
    //    event.preventDefault();
    //    $('#modalWaiting').modal('show');
    //    setTimeout(function () {
    //        $('#modalWaiting').on('hidden.bs.modal', function () {
    //            window.location = create;
    //        }).modal('hide')
    //    }, 500);
    //});
    $("#btntest").click(function (event) {
        event.preventDefault();
        $('#modaltest').modal('show');
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
    //test

   



   




















});