namespace GoodOnYouScrapperAPI.Models;

public class BrandModel
{
    private int environmentRating;
    private int peopleRating;
    private int animalRating;
    
    
    public BrandModel (int environmentRating, int peopleRating, int animalRating)
    {
        this.environmentRating = environmentRating;
        this.peopleRating = peopleRating;
        this.animalRating = animalRating;
    }

    public string ToString()
    {
        return "Environment: " + environmentRating + " People: " + peopleRating + " Animal: " + animalRating;
    }
}