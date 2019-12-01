var input = document.querySelector('.tree');
var collapseNavMenu = false;

if (input) {
    input.addEventListener('click', t => {

        const { target } = t;

        console.log(target);
        if (
            (((target.nodeName === 'SPAN' &&
                target.classList.contains('treeLabel'))
                ||
                (target.nodeName === 'SPAN' &&
                    target.classList.contains('treeSwitcher')))
                &&
                $(target).css('cursor') != 'not-allowed')
        ) {
            onSwitcherClick(target);
        }
    });
}

function toggleNavMenu() {
    collapseNavMenu = !collapseNavMenu;

    collapseNavMenu ? $('#toggleNavMenu').addClass('collapse') : $('#toggleNavMenu').removeClass();
}

function onSwitcherClick(target) {
    const liEle = target.parentNode;
    const ele = liEle.lastChild;

    if (liEle.classList.contains('treeNodeClose')) {
        liEle.classList.remove('treeNodeClose');
        $(ele).slideDown(500);
    } else {
        $(ele).slideUp(500);
        liEle.classList.add('treeNodeClose');
    }
};

function addNode(parentId) {

    var node = {
        name: $('#addNodeName' + parentId).val(),
        parentId: parentId,
    };

    $.ajax({
        url: 'https://localhost:44349/api/node/add',
        contentType: 'application/json;charset=utf-8',
        type: 'POST',
        data: JSON.stringify(node),
        success: function() {
            location.reload()
        },
        error: function(response) {
            console.log(response.responseText);
        }
    });
}

function addNodeInput(parentId) {
    var input = $('#addNodeSpan' + parentId);

    if (input.css('display') == 'none') {
        input.css('display', 'flex');
    } else {
        input.css('display', 'none');
    }
}

function editNodeInput(id) {
    var input = $('#editNodeSpan' + id);

    if (input.css('display') == 'none') {
        input.css('display', 'flex');
    } else {
        input.css('display', 'none');
    }
}

function updateNode(id) {

    var node = {
        name: $('#editNodeName' + id).val(),
        parentId: parseInt($('#editNodeParentId' + id).val()),
    };

    $.ajax({
        url: 'https://localhost:44349/api/node/update/' + id,
        contentType: 'application/json;charset=utf-8',
        type: 'PUT',
        data: JSON.stringify(node),
        success: function() {
            location.reload()
        },
        error: function(response) {
            console.log(response.responseText);
        }
    });
}

function deleteNode(id) {

    $.ajax({
        url: 'https://localhost:44349/api/node/' + id,
        contentType: 'application/json;charset=utf-8',
        type: 'DELETE',
        success: function() {
            location.reload()
        },
        error: function(response) {
            console.log(response.responseText);
        }
    });
}