﻿using UnityEngine;
using System.Collections;

public abstract class AbilityWord : Word
{
	protected string[] wordTiers;
	protected int currentTier = 0;

	[Range(0.0f, 300.0f)]public float wordCooldown = 10.0f;
	private float currentCooldown;

	[SerializeField] protected WordHUD wordHUD = null;

	private const int pixels = 32;
	protected float pixelCooldown;

	protected override void Start ()
	{
		pixelCooldown =(1.0f / (float)pixels) * 1000.0f;
		word = wordTiers [0];
		currentTier = 0;
		currentCooldown = wordCooldown;
		wordHUD.Deactivate ();
		base.Start ();
	}

	protected override void Update()
	{
		if(!behaviorActive)
		{
			currentCooldown = Mathf.Min(currentCooldown + Time.deltaTime, wordCooldown);
			if(wordCooldown == 0.0f)
			{
				wordHUD.UpdateCooldown (1.0f);
			}
			else
			{
				float _cooldownPercent = currentCooldown / wordCooldown;
				wordHUD.UpdateCooldown (((_cooldownPercent * 1000.0f) - ((_cooldownPercent * 1000.0f) % pixelCooldown)) / 1000.0f);
			}
		}
		base.Update ();
	}

	protected override void TriggerBehavior ()
	{
		//Activate behavior
		if(!behaviorActive)
		{
			currentCooldown = 0.0f;
			wordHUD.TriggerSuccess();
			VisualCommandPanel.instance.AddMessage("Running " + wordTiers[currentTier]);
			base.TriggerBehavior();
		}
		//Behavior on cooldown, do something to show this (flicker red?)
		else
		{
			VisualCommandPanel.instance.AddMessage(wordTiers[currentTier] + " already running");
		}
	}

	protected override void Behavior ()
	{
	}

	protected override void EndBehavior()
	{
		base.EndBehavior ();
	}

	public void SetTier(int _tier)
	{
		currentTier = _tier;
		if(currentTier == 0)
		{
			wordActive = true;
			wordHUD.Activate();
		}
		thisWord = wordTiers [currentTier];
		wordHUD.UpdateWord (thisWord);
	}


}