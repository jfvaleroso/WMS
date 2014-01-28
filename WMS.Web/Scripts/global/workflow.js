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

    var redirectAction = $("#RedirectAction").val();
    var rearrangeWorkflow = $("#RearrangeWorkflow").val();
    var addWorkflow = $("#AddWorkflow").val();

    var add = $("#Add").val();

    //workflow
    var getSubProcessByProcessId = $("#GetSubProcessByProcessId").val();
    var getAllRolesByProcessId = $("#GetAllRolesByProcessId").val();
    var getClassificationBySubProcessId = $("#GetClassificationBySubProcessId").val();

   
    //workflow
    $(document).on("change", "#ProcessId", function (event) {
    //$("#ProcessId").on("change", function (event) {
        event.preventDefault();
        $("#ClassificationId").select2('data', null);
        var id = $("#ProcessId").val() != "" ? $("#ProcessId").val() : "0";
        $.ajax({
            url: getSubProcessByProcessId,
            type: "POST",
            data: { filter: id, selectedValue: '0' },
            beforeSend: function () {
            },
            complete: function () {
            },
            error: function (xhr, textStatus, error) {
                alert(error);
            },
            success: function (data) {
                $("#partial-subProcess").html(data);
                $("#partial-subProcess .combobox").select2({
                    placeholder: "Select one",
                    allowClear: true
                });
            }
        });

        $.ajax({
            url: getAllRolesByProcessId,
            type: "POST",
            data: { id: id },
            beforeSend: function () {
            },
            complete: function () {
            },
            error: function (xhr, textStatus, error) {
                alert(error);
            },
            success: function (data) {
                $("#partial-requestor").html(data);
                $("#partial-requestor .combobox").select2({
                    placeholder: "Select one",
                    allowClear: true
                });
            }
        });
    });

    $(document).on("change", "#SubProcessId", function (event) {
    //$("#SubProcessId").on("change", function (event) {
        event.preventDefault();
        var id = $("#SubProcessId").val() != "" ? $("#SubProcessId").val() : "0";
        $.ajax({
            url: getClassificationBySubProcessId,
            type: "POST",
            data: { filter: id, selectedValue: '0' },
            complete: function (data) {
            },
            error: function (xhr, textStatus, error) {
                alert(xhr.statusText);
            },
            success: function (data) {
                $("#partial-classification").html(data);
                $("#partial-classification .combobox").select2({
                    placeholder: "Select one",
                    allowClear: true
                });
            }
        });
    });
   

    //workflow mapping
    function activateTableDND() {
        $("#WorkflowTable").tableDnD();
        $("#WorkflowTable").tableDnD({
            onDragClass: "tDnD_whileDrag",
            onDrop: function (table, row) {
                var rows = table.tBodies[0].rows;
                var ids = "";
                for (var i = 0; i < rows.length; i++) {
                    if (i == (--rows.length))
                    { ids += rows[i].id; }
                    else
                    { ids += rows[i].id + ","; }

                }


                $.ajax({
                    url: rearrangeWorkflow,
                    type: "post",
                    data: { id: ids },
                    complete: function (data) {
                    },
                    error: function (data) {
                        alert('error');
                    },
                    success: function (data) {
                        $('#data-list').html(data);
                        $('#data-list').tooltip({
                            selector: "[data-toggle=tooltip]",
                            container: "body"
                        })
                        activateTableDND();
                    }
                });

            },

        });
    }

    function clearForm() {
        $("#Assignee, #NoticeTo, #Operator").select2('data', null);
        $("#SLA").val("");
    }

    $("#btnAddLevel").on('click', function (event) {
        event.preventDefault();
        $.ajax({
            url: addWorkflow,
            type: "post",
            data: $('form').serialize(),
            complete: function (data) {
            },
            error: function (data) {
                alert('error');
            },
            success: function (data) {

                $('#data-list').html(data);
                $('#data-list').tooltip({
                    selector: "[data-toggle=tooltip]",
                    container: "body"
                })
                activateTableDND();
                clearForm();
            }
        });
    });
    function getMaximumSelectionSize(no) {
        $("#Assignee").select2({
            maximumSelectionSize: no
        });
        $("#NoticeTo").select2({
            maximumSelectionSize: no
        });
    }

    getMaximumSelectionSize(1);
    $("#options").change(function (event) {
        event.preventDefault();
        var options = $("#options").val();
        clearForm();
        if ($(this)[0].options[1].selected) {
            $("#operator-container").removeClass('hide');
            $("#note-assignee").empty().append('Specify multiple approvers at current level.');
            getMaximumSelectionSize(10);
        }
        if ($(this)[0].options[0].selected) {
            $("#operator-container").addClass('hide');
            $("#note-assignee").empty().append('Specify single approver at current level');
            $("#Assignee").select2('data', null);
            getMaximumSelectionSize(1);
        }

    });

    //document mapping// workflow mapping// notificaiton mapping
    $("#btnAdd").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: add,
            type: "post",
            data: $('form').serialize(),
            complete: function (data) {
            },
            error: function (data) {
            },
            success: function (data) {

                $('#data-list').replaceWith(data);
                $('#data-list').tooltip({
                    selector: "[data-toggle=tooltip]",
                    container: "body"
                })

            }
        });
    });


    //main workflow transaction
    // post: save
    $("#btnSaveAndNext").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: create,
            type: "post",
            cache: false,
            data: $('form').serialize(),
            beforeSend: function () {
                $('form').validate().form();
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
                    if (redirectAction != '' && data.result != '') {
                        window.location = redirectAction + "/Index/" + data.result;
                    }
                    else {
                        window.location = redirectAction;
                    }

                }
                    //invalid
                else if (data.code == "007") {
                    $('#alert').removeClass('hide');
                    $('#general-status').empty().append('Required field!');
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
                    $('#general-status').empty().append('Error has occured!' + data.message);
                }

            }
        });
    });
    // skip
    $("#btnSkip").click(function (event) {
        event.preventDefault();
        window.location = redirectAction + "/Index/" + $("#SecuredId").val();
    });

    // post: save changes
    $("#btnModify").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: manage,
            type: "post",
            cache: false,
            data: $('form').serialize(),
            beforeSend: function () {
                $('form').validate().form();
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
                    $('#general-status').empty().append('Required field!');
                }
                    //existed
                else if (data.code == "003") {
                    $('#alert').removeClass('hide');
                    $('#general-status').empty().append('Item already exists!');
                }
                    //error
                else if (data.code == "005") {
                    $('#alert').removeClass('hide');
                    $('#general-status').empty().append('Error has occured!' + data.message);
                }

            }
        });
    });

    



});