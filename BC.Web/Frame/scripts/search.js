$(function () {
    // Execute search if user clicks enter
    $("#place-field").keyup(function (event) {
        
        Search();

        if (event.keyCode == 13) {
            SearchPlace($('#place-hidden').val(), $("#place-field").val());
        }

        $('#place-hidden').val('');
    });

    var sort = $('#sort-hidden').val();
    if (sort && sort.length > 0)
    {
        $('#ddSort').val(sort);
    }
});

function Search() {

    var q = $("#place-field").val();

    $.post('/search/search',
    {
        q: q
    },
    function (data) {
        var searchResultsHTML = $("#result-places").html('');
        var host = $('#place-hidden').attr('data-host');

        var places = [];
        if (host.indexOf("mowido.se") >= 0) {
            for (var i = 0; i < data.length; i++) {
                var obj = { value: data[i].Document.ID, label: data[i].Document.NameSV }
                places.push(obj);
            };
        }
        else {
            for (var i = 0; i < data.length; i++) {
                var obj = { value: data[i].Document.ID, label: data[i].Document.NameSV }
                places.push(obj);
            };
        }


        $("#place-field").autocomplete({
            minLength: 0,
            source: places,
            focus: function (event, ui) {
                $("#place-field").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#q").val(ui.item.label);
                $("#place-hidden").val(ui.item.value);
                SearchPlace(ui.item.value);
                return false;
            },
            open: function (result) {

                if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                    $('.ui-autocomplete').off('menufocus hover mouseover mousemove');
                }
            }
        })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div>" + item.label + "</div>")
      .appendTo(ul);
};
    });



    function parseJsonDate(jsonDateString) {
        if (jsonDateStrinSg != null)
            return new Date(parseInt(jsonDateString.replace('/Date(', '')));
        else
            return "";
    }
};

function SearchPlace(p, q) {
    var hsearch = true;
    if (p.length > 0) {
        var url = "http://" + $('#place-hidden').attr('data-host') + "/" + $('#place-hidden').attr('data-startslug') + "/" + p + "/placeslug";
        hsearch = false;
        window.location.href = url;
    }

    if (q.length > 0 && hsearch)
    {
        $.post('/search/search',
        {
            q: q
        },
        function (data) {
            var url = "http://" + $('#place-hidden').attr('data-host') + "/";

            if(data[0])
            {
                url += $('#place-hidden').attr('data-startslug') + "/" + data[0].Document.ID + "/placeslug";
            }

            window.location.href = url;
        });
     }
}

$(document).ready(function () {
    $('.btn-search').click(function () {
        var p = $('#place-hidden').val();
        var q = $('#place-field').val();

        SearchPlace(p, q);
    });
});

function sortSearch(q)
{
    $.post('/search/sortsearch',
        {
            q: q,
            filter: $('#filter-hidden').val(),
            id: $('#place-hidden').val(),
            slug: $('#place-field').val(),
            startslug: $('#place-hidden').attr('data-startslug')
        }, function (data) {

            if (data) {
                window.location.href = data;
            }
        });
}

function ddFilterChange() {
    var result;
    var filter = $('#filter-hidden');

    var minPrice = $('#ddPriceMin').val();
    var maxPrice = $('#ddPriceMax').val();

    if (minPrice && maxPrice)
    {
        result = minPrice + '_' + maxPrice;

    }

    console.log(result);

    filter.val(result);
}
