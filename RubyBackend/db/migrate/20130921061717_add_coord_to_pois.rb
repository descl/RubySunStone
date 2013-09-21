class AddCoordToPois < ActiveRecord::Migration
  def change
    add_column :pois, :coordX, :double
    add_column :pois, :coordY, :double
  end
end
