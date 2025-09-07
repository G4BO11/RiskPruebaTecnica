class VentaManager {
  constructor() {
    this.productosEnVenta = [];
    this.clienteSeleccionado = null;
    this.totalVenta = 0;
    this.initializeEvents();
  }

  initializeEvents() {
    $("#buscarCliente").click(() => this.buscarCliente());
    $("#numeroIdentificacion").keypress((e) => {
      if (e.which === 13) this.buscarCliente();
    });
    $("#registrarCliente").click(() => this.mostrarModalCliente());
    $("#guardarCliente").click(() => this.guardarClienteNuevo());
    $("#agregarProducto").click(() => this.agregarProducto());
    $("#procesarVenta").click(() => this.procesarVenta());

    $("#productoSelect").change(() => this.validarStockProducto());
    $("#cantidad").keyup(() => this.validarStockProducto());
  }

  async buscarCliente() {
        const numeroIdentificacion = $('#numeroIdentificacion').val().trim();
        
        if (!numeroIdentificacion) {
            alert('Ingrese un número de identificación');
            return;
        }

        try {
            const response = await fetch(`/Ventas/BuscarCliente?numeroIdentificacion=${numeroIdentificacion}`);
            const result = await response.json();

            if (result.found) {
                this.mostrarClienteEncontrado(result.cliente);
            } else {
                this.mostrarClienteNoEncontrado(numeroIdentificacion);
            }
        } catch (error) {
            console.error('Error buscando cliente:', error);
            alert('Error al buscar cliente');
        }
    }
   
  mostrarClienteEncontrado(cliente) {
    this.clienteSeleccionado = cliente;
    $("#clienteId").val(cliente.id);
    $("#clienteNombre").text(cliente.nombreCompleto);
    $("#clienteInfo").show();
    $("#clienteNoEncontrado").hide();
    this.validarFormularioVenta();
  }

  mostrarClienteNoEncontrado(numeroIdentificacion) {
    $("#nuevoNumeroIdentificacion").val(numeroIdentificacion);
    $("#clienteInfo").hide();
    $("#clienteNoEncontrado").css("display", "flex").show();
    this.clienteSeleccionado = null;
    this.validarFormularioVenta();
  }

  mostrarModalCliente() {
    const numeroIdentificacion = $("#numeroIdentificacion").val();
    $("#nuevoNumeroIdentificacion").val(numeroIdentificacion);
    $("#modalClienteNuevo").modal("show");
  }

  async guardarClienteNuevo() {
    const clienteData = {
      NumeroIdentificacion: $("#nuevoNumeroIdentificacion").val(),
      Nombre: $("#nuevoNombre").val(),
      Apellido: $("#nuevoApellido").val(),
      Direccion: $("#nuevaDireccion").val(),
      Telefono: $("#nuevoTelefono").val(),
      Email: $("#nuevoEmail").val(),
    };

    if (
      !clienteData.Nombre ||
      !clienteData.Apellido ||
      !clienteData.NumeroIdentificacion
    ) {
      alert("Complete los campos obligatorios");
      return;
    }

    try {
      const response = await fetch("/Ventas/CrearClienteRapido", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          RequestVerificationToken: $(
            'input[name="__RequestVerificationToken"]'
          ).val(),
        },
        body: JSON.stringify(clienteData),
      });

      const result = await response.json();

      if (result.success) {
        this.mostrarClienteEncontrado(result.cliente);
        $("#modalClienteNuevo").modal("hide");
        this.limpiarFormularioCliente();
        alert("Cliente registrado exitosamente");
      } else {
        alert("Error: " + result.error);
      }
    } catch (error) {
      console.error("Error guardando cliente:", error);
      alert("Error al guardar cliente");
    }
  }


  async validarStockProducto() {
    const productoId = $("#productoSelect").val();
    const cantidad = parseInt($("#cantidad").val()) || 1;

    if (!productoId) return;

    try {
      const response = await fetch(
        `/Ventas/ValidarStock?productoId=${productoId}&cantidad=${cantidad}`
      );
      const result = await response.json();

      const btnAgregar = $("#agregarProducto");
      if (result.tieneStock) {
        btnAgregar
          .prop("disabled", false)
          .removeClass("btn-danger")
          .addClass("btn-success");
      } else {
        btnAgregar
          .prop("disabled", true)
          .removeClass("btn-success")
          .addClass("btn-danger");
        alert(`Stock insuficiente. Disponible: ${result.stockDisponible}`);
      }
    } catch (error) {
      console.error("Error validando stock:", error);
    }
  }

  async agregarProducto() {
    const productoId = $("#productoSelect").val();
    const cantidad = parseInt($("#cantidad").val()) || 1;

    if (!productoId) {
      alert("Seleccione un producto");
      return;
    }

    const productoExistente = this.productosEnVenta.find(
      (p) => p.productoId === productoId
    );
    if (productoExistente) {
      alert(
        "El producto ya está en la venta. Use la opción de editar cantidad."
      );
      return;
    }

    try {
      const response = await fetch(
        `/Ventas/ObtenerProducto?productoId=${productoId}`
      );
      const result = await response.json();

      if (result.found) {
        const producto = result.producto;
        const subtotal = producto.precio * cantidad;

        const itemVenta = {
          productoId: producto.id,
          nombre: producto.nombre,
          precio: producto.precio,
          cantidad: cantidad,
          subtotal: subtotal,
        };

        this.productosEnVenta.push(itemVenta);
        this.actualizarTablaProductos();
        this.limpiarSeleccionProducto();
        this.validarFormularioVenta();
      }
    } catch (error) {
      console.error("Error agregando producto:", error);
      alert("Error al agregar producto");
    }
  }

  actualizarTablaProductos() {
    const tbody = $("#productosVenta tbody");
    tbody.empty();

    this.totalVenta = 0;

    this.productosEnVenta.forEach((item, index) => {
      this.totalVenta += item.subtotal;

      const row = `
                <tr>
                    <td>${item.nombre}</td>
                    <td>$${item.precio.toLocaleString()}</td>
                    <td>${item.cantidad}</td>
                    <td>$${item.subtotal.toLocaleString()}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-danger" onclick="ventaManager.eliminarProducto(${index})">
                            Eliminar
                        </button>
                    </td>
                </tr>
            `;
      tbody.append(row);
    });

    $("#totalVenta").text(`$${this.totalVenta.toLocaleString()}`);
  }

  eliminarProducto(index) {
    this.productosEnVenta.splice(index, 1);
    this.actualizarTablaProductos();
    this.validarFormularioVenta();
  }

  async procesarVenta() {
    if (!this.clienteSeleccionado || this.productosEnVenta.length === 0) {
      alert("Complete todos los datos de la venta");
      return;
    }

    const ventaData = {
      ClienteId: this.clienteSeleccionado.id,
      Detalles: this.productosEnVenta.map((item) => ({
        ProductoId: item.productoId,
        Cantidad: item.cantidad,
      })),
    };

    try {
      $("#procesarVenta").prop("disabled", true).text("Procesando...");

      const response = await fetch("/Ventas/CreateJson", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          RequestVerificationToken: $(
            'input[name="__RequestVerificationToken"]'
          ).val(),
        },
        body: JSON.stringify(ventaData),
      });
      console.log(ventaData);

      const result = await response.json();

      if (result.success) {
        alert(result.message);
        window.location.href = "/Ventas";
      } else {
        alert("Error: " + result.error);
      }
    } catch (error) {
      console.error("Error procesando venta:", error);
      alert("Error al procesar venta");
    } finally {
      $("#procesarVenta").prop("disabled", false).text("Procesar Venta");
    }
  }

  validarFormularioVenta() {
    const tieneCliente = this.clienteSeleccionado !== null;
    const tieneProductos = this.productosEnVenta.length > 0;

    $("#procesarVenta").prop("disabled", !tieneCliente || !tieneProductos);
  }

  limpiarSeleccionProducto() {
    $("#productoSelect").val("");
    $("#cantidad").val(1);
    $("#agregarProducto")
      .prop("disabled", false)
      .removeClass("btn-danger")
      .addClass("btn-success");
  }

  limpiarFormularioCliente() {
    $("#formClienteNuevo")[0].reset();
  }
}

$(document).ready(function () {
  window.ventaManager = new VentaManager();
});
