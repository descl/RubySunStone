class Poi < ActiveRecord::Base
  def self.newFromArray(tab,kind)
    return  Poi.new(:x => tab[0],:y => tab[1], :kind => kind)
  end
end
