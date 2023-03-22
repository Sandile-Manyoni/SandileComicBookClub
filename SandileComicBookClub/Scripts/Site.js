$("#changePW").click(function () {
    $("#PWmenu").show("Slow");
    $("#changed").attr("Value", 1)
})

$("#cancelChangePW").click(function () {
    $("#PWmenu").hide("Slow");
    $("#changed").attr("Value", 0)
})