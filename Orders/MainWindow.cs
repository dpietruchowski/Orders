using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	Orders.ProductsDialog products;
	Orders.OrdersDialog orders;
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		notebook.PageAdded += (object o, PageAddedArgs args) => {
			args.P0.Show();
			notebook.CurrentPage = (int)args.P1;
		};
		products = new Orders.ProductsDialog ();
		products.Hide();
		orders = new Orders.OrdersDialog ();
		orders.Hide();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	private void AddProductTab (int id, string name)
	{
		var widget = new Orders.ProductWidget ();
		widget.Init (id);
		widget.Products = products;
		AddTab (widget, name);
	}

	private void AddOrderTab (int id, string name)
	{
		var widget = new Orders.OrdeWidget ();
		widget.Init (id);
		widget.Products = products;
		AddTab (widget, name);
	}

	private void AddTab(Widget widget, string name)
	{
		notebook.Add (widget);
		HBox hbox = new HBox();
		hbox.PackStart(new Label(name) );
		Button close = new Button("×"); // Set this up with an image or whatever.
		close.Relief = ReliefStyle.None;
		close.FocusOnClick = false;
		close.Clicked += delegate {
			hbox.Destroy();
			widget.Destroy();
		};

		hbox.PackStart(close);
		hbox.ShowAll();
		notebook.SetTabLabel(widget, hbox);
	}
	
	protected void OnNewActionActivated (object sender, EventArgs e) // Product
	{
		var id = Orders.Database.ProductTable.NewId ();
		AddProductTab (id, "New Product");

	}
	protected void OnOpenActionActivated (object sender, EventArgs e) // Product
	{
		products.Show();
		products.Run ();
			
		var selected = products.Selected;
		if (selected != null) {
			AddProductTab (selected.Id, "Product");
		}

		products.Hide ();
	}
	
	protected void OnNewAction1Activated (object sender, EventArgs e) // Order
	{
		var id = Orders.Database.OrderTable.NewId ();
		AddOrderTab (id, "New Order");
	}
	protected void OnOpenAction1Activated (object sender, EventArgs e) // Order
	{
		orders.ShowAndRefresh();
		orders.Run ();

		var selected = orders.Selected;
		if (selected != null) {
			AddOrderTab (selected.Id, "Order");
		}
		
		orders.Hide();
	}
}
