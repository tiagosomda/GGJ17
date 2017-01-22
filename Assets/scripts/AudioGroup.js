#pragma strict

RequireComponent(AudioSource);

class AudioGroup extends MonoBehaviour {

	enum Mode {
		random,
		sequence
	}

	public var source: AudioSource;
	public var clips: AudioClip[];
	public var playMode: Mode= Mode.random;

	private var index: int;

	function Create (array: Array) {

		this.clips= array;

	}

	function Start () {

		if (!source) {
			source= gameObject.AddComponent(AudioSource) as AudioSource;
		}

		index= Random.Range(0, clips.length);

	}


	function Update () {
	}

	public function Play() {

		source.clip= clips[index];
		source.Play();

		if (playMode == Mode.random) {
			index= Random.Range(0, clips.length);
		}
		else if (playMode == Mode.sequence) {
			index++;
			index= index%clips.length;
		}
	}
}
