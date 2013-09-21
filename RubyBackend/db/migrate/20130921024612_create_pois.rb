class CreatePois < ActiveRecord::Migration
  def change
    create_table :pois do |t|
      t.integer :x
      t.integer :y
      t.integer  :kind
      t.integer :lvl

      t.timestamps
    end
  end
end
