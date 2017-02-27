using System.IO;
using System.Threading.Tasks;
using AudioManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentificationServer.Controllers
{
    [Route("api/[controller]")]
    public class AudioController : Controller
    {
        private AudioRepository _audioRepo;


        public AudioController(AudioRepository audioRepo)
        {
            _audioRepo = audioRepo;
        }

        public async Task<IActionResult> IdentifyAudio(string radio, string id)
        {
            var path = _audioRepo.GetAudioPath(Path.Combine(radio, id));
            var audio = _audioRepo.GetAudioList(path);

            //TODO: Remove hardcode
            await _audioRepo.UploadAsync(path);

            if (audio != null)
            {
                return Ok();
            }
            return NotFound();
        }

        public async Task<IActionResult> Download(string radio, string id)
        {
            var path = Path.Combine(radio, id);
            try
            {
                await _audioRepo.DownloadAsync(path);
                return Ok();
            }
            catch (Google.GoogleApiException e)
            {
                return new JsonResult(e);
            }
        }

        public async Task<IActionResult> Upload(string radio, string id)
        {
            var path = Path.Combine(radio, id);
            try
            {
                await _audioRepo.UploadAsync(path);
                return Ok();
            }
            catch (Google.GoogleApiException e)
            {
                return new JsonResult(e);
            }
        }
    }
}
